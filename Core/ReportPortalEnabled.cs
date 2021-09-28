using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortalNUnitLog4netClient.Core.Logging;
using ReportPortalNUnitLog4netClient.Models;

namespace ReportPortalNUnitLog4netClient.Core
{
    internal class ReportPortalEnabled : IReportPortalService
    {
        private readonly object _lockObj = new object();
        private const string ProductBug = "PB001";
        private readonly IDictionary<Type, string> _defectsResolutions = new Dictionary<Type, string>();

        private readonly Service _service;
        private readonly RpConfiguration _rpConfiguration;
        private static ConcurrentDictionary<string, Tuple<string, string, string, string>> _tests;
        private static ConcurrentDictionary<string, string> _suites;
        private static ConcurrentDictionary<string, string> _subSuites;
        private static ConcurrentDictionary<string, string> _parentTestItem;
        private static ConcurrentDictionary<string, int> _countOfTheTestsExecutions;
        private static readonly string RpDataFileName = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}rp_data.txt";

        private Launch Launch { get; set; }

        public ReportPortalEnabled(RpConfiguration rpConfiguration, IDictionary<Type, string> defectsMapping = null)
        {
            _rpConfiguration = rpConfiguration;
            _service = new Service(_rpConfiguration.Uri, _rpConfiguration.Project, _rpConfiguration.Uuid);
            _tests = new ConcurrentDictionary<string, Tuple<string, string, string, string>>();
            _suites = new ConcurrentDictionary<string, string>();
            _subSuites = new ConcurrentDictionary<string, string>();
            _parentTestItem = new ConcurrentDictionary<string, string>();
            _countOfTheTestsExecutions = new ConcurrentDictionary<string, int>();
            if (defectsMapping != null)
            {
                _defectsResolutions = defectsMapping;
            }
        }
        public IReportPortalService StartLaunch(List<string> tags = null)
        {
            var startTime = DateTime.UtcNow;
            var launch = Task.Run(async () => await _service.StartLaunchAsync(new StartLaunchRequest
            {
                Name = _rpConfiguration.LaunchName,
                StartTime = startTime,
                Mode = _rpConfiguration.Mode,
                Tags = tags
            })).Result;
            launch.StartTime = startTime;
            Launch = launch;
            return this;
        }

        public IReportPortalService FinishLaunch()
        {
            var isForceFinish = false;
            try
            {
                FinishItems(_parentTestItem);
                FinishItems(_subSuites);
                FinishItems(_suites);
            }
            catch (Exception)
            {
                isForceFinish = true;
            }
            finally
            {
                Task.Run(async () =>
                {
                    var endTime = DateTime.UtcNow;
                    Launch.EndTime = endTime;
                    await _service.FinishLaunchAsync(Launch.Id, new FinishLaunchRequest { EndTime = endTime }, isForceFinish);
                }).Wait();
                if (_rpConfiguration.IsWriteLaunchData)
                {
                    WriteLaunchDataToFile();
                }
            }
            return this;
        }

        public IReportPortalService StartTest(TestContext.TestAdapter test, string suiteName, string subSuite, List<string> tags, string testCodeId = null, List<string> tmsIds = null)
        {
            lock (_lockObj)
            {
                //check if suite is started
                if (!_suites.ContainsKey(suiteName))
                {
                    var suiteItem = _service.StartTestItemAsync(new StartTestItemRequest
                    {
                        LaunchId = Launch.Id,
                        Name = suiteName,
                        StartTime = DateTime.UtcNow,
                        Type = TestItemType.Suite
                    }).Result;
                    _suites.GetOrAdd(suiteName, suiteItem.Id);
                }
            }

            var suitePlusSubSuiteKey = suiteName + subSuite;
            lock (_lockObj)
            {
                //check if subSuite is started
                if (!_subSuites.ContainsKey(suitePlusSubSuiteKey))
                {
                    var suiteId = _suites[suiteName];
                    var subSuiteItem = _service.StartTestItemAsync(suiteId, new StartTestItemRequest
                    {
                        LaunchId = Launch.Id,
                        Name = subSuite,
                        StartTime = DateTime.UtcNow,
                        Type = TestItemType.Suite,
                    }).Result;
                    _subSuites.GetOrAdd(suitePlusSubSuiteKey, subSuiteItem.Id);
                }
            }

            var parentItemName = suitePlusSubSuiteKey + GetParentItemName(test);
            lock (_lockObj)
            {
                //check if parent item is started
                if (!_parentTestItem.ContainsKey(parentItemName))
                {
                    var subSuiteId = _subSuites[suitePlusSubSuiteKey];
                    var parentItem = _service.StartTestItemAsync(subSuiteId, new StartTestItemRequest
                    {
                        LaunchId = Launch.Id,
                        Name = GetParentItemName(test),
                        StartTime = DateTime.UtcNow,
                        Type = TestItemType.Suite,
                        Tags = tags,
                        Description = GetTestParentDescription(testCodeId, tmsIds)
                    }).Result;
                    _parentTestItem.GetOrAdd(parentItemName, parentItem.Id);
                }
            }

            var testName = GetTestName(test);
            var parentId = _parentTestItem[parentItemName];
            var testItem = _service.StartTestItemAsync(parentId, new StartTestItemRequest
            {
                LaunchId = Launch.Id,
                Name = testName,
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Test,
                Description = GetTestDataDescription(test)
            }).Result;

            var resultItem = new Tuple<string, string, string, string>(testItem.Id, parentId, _subSuites[suitePlusSubSuiteKey], _suites[suiteName]);
            _tests.GetOrAdd(test.ID, resultItem);
            return this;
        }

        public IReportPortalService FinishTest(TestContext.TestAdapter test, TestStatus testStatus, string errorMessage, List<string> ticketIds)
        {
            var testId = test.ID;
            if (!_tests.ContainsKey(testId))
            {
                return this;
            }
            var testItem = _tests[testId];

            var status = testStatus.ToRpStatus();
            if (status != Status.Passed)
            {
                var request = new AddLogItemRequest
                {
                    TestItemId = testItem.Item1,
                    Level = LogLevel.Error,
                    Time = DateTime.UtcNow,
                    Text = errorMessage
                };
                var result = _service.AddLogItemAsync(request).Result;
            }

            var issue = GetIssue(status, ticketIds, out bool isProductBug);
            var message = _service.FinishTestItemAsync(testItem.Item1, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = status,
                Issue = issue,
                Description = BuildDescriptionForTheTestItem(ticketIds, issue, isProductBug)
            }).Result;

            return this;
        }

        public IReportPortalService Log(TestContext.TestAdapter test, LogLevel level, DateTime time, string text, Attach attach = null)
        {
            var testId = test.ID;
            if (!_tests.ContainsKey(testId))
            {
                return this;
            }
            var testItem = _tests[testId];
            var request = new AddLogItemRequest
            {
                TestItemId = testItem.Item1,
                Level = level,
                Time = time,
                Text = text,
                Attach = attach
            };
            var result =_service.AddLogItemAsync(request).Result;
            return this;
        }

        public Launch GetLaunch()
        {
            return Launch;
        }

        private string GetParentItemName(TestContext.TestAdapter testAdapter)
        {
            return testAdapter.MethodName;
        }

        private string GetTestName(TestContext.TestAdapter testAdapter)
        {
            var testArguments = testAdapter.Arguments;
            var testName = GetTestNameShort(testAdapter);
            if (testArguments.Length == 0)
            {
                return testName;
            }

            if (!_countOfTheTestsExecutions.ContainsKey(testName))
            {
                _countOfTheTestsExecutions.GetOrAdd(testName, 1);
            }
            else
            {
                var incrementedCounter = _countOfTheTestsExecutions[testName] + 1;
                _countOfTheTestsExecutions[testName] = incrementedCounter;
            }
            return $"{testName}_{_countOfTheTestsExecutions[testName]}";
        }

        private string GetTestNameShort(TestContext.TestAdapter testAdapter)
        {
            return testAdapter.MethodName;
        }

        private void FinishItems(ConcurrentDictionary<string, string> items)
        {
            foreach (var pair in items)
            {
                var message = _service.FinishTestItemAsync(pair.Value, new FinishTestItemRequest
                {
                    EndTime = DateTime.UtcNow,
                }).Result;
            }
        }

        private Issue GetIssue(Status status, ICollection<string> ticketIds, out bool isProductBug)
        {
            if (status != Status.Failed)
            {
                isProductBug = false;
                return null;
            }

            var errorMessage = TestContext.CurrentContext.Result.Message;
            var noTickets = ticketIds.Count == 0;

            if (noTickets && errorMessage != null)
            {
                var resolutionResults = _defectsResolutions.FirstOrDefault(p => errorMessage.Contains(p.Key.Name));
                if (!resolutionResults.Equals(default(KeyValuePair<Type, string>)))
                {
                    isProductBug = false;
                    return new Issue
                    {
                        Type = resolutionResults.Value,
                        Comment = errorMessage,
                        AutoAnalyzed = false,
                        IgnoreAnalyzer = true,
                    };
                }
            }
            if (noTickets)
            {
                isProductBug = false;
                return null;
            }
            //otherwise build ProductBug
            isProductBug = true;
            var comment = BuildIssuesDescriptions(ticketIds);

            var issue = new Issue
            {
                Type = ProductBug,
                Comment = comment,
                AutoAnalyzed = false,
                IgnoreAnalyzer = true,
            };
            return issue;
        }

        private string BuildDescriptionForTheTestItem(ICollection<string> ticketIds, Issue issue, bool isProductBug)
        {
            var description = isProductBug ? BuildIssuesDescriptions(ticketIds) : "Automatic resolution for the issue has been set";
            return issue == null ? null : description;
        }

        private string BuildIssuesDescriptions(ICollection<string> ticketIds)
        {
            if (ticketIds.Count == 0)
            {
                return null;
            }

            return $"Issue(s):{Environment.NewLine}" + ticketIds.Select(t =>
            {
                if (_rpConfiguration.IssueUri == null)
                {
                    return t;
                }
                return _rpConfiguration.IssueUri + t;
            }).Aggregate((first, next) =>  $"{first};{Environment.NewLine}{next}");
        }

        private string GetTestParentDescription(string testCaseId = null, IReadOnlyCollection<string> testCaseTicketIds = null)
        {
            var testCaseString = testCaseId != null ? $"Id:{testCaseId}": null;
            var testCaseTicketString = testCaseTicketIds != null ? 
                string.Join($",{Environment.NewLine}", testCaseTicketIds.Select(s => $"TmsUri:{_rpConfiguration.TestManagementSystemUri + s}")) : null;

            return $"{testCaseString}{Environment.NewLine}{testCaseTicketString}";
        }

        private string GetTestDataDescription(TestContext.TestAdapter testAdapter)
        {
            return testAdapter.Arguments.Length == 0 ? null : $"InputData:{Environment.NewLine}{DecorateArguments(testAdapter.Arguments)}";
        }

        private void WriteLaunchDataToFile()
        {
            if (File.Exists(RpDataFileName))
            {
                File.Delete(RpDataFileName);
            }
            var resultData = $"{Launch?.Id},{Launch?.StartTimeString},{Launch?.EndTimeString}";
            File.WriteAllText(RpDataFileName, resultData);
        }

        private string DecorateArguments(IEnumerable<object> arguments)
        {
            var result = "[";
            foreach (var item in arguments)
            {
                result += item;
                result += ",";
            }
            return result.Remove(result.Length - 1) + "]";
        }
    }
}
