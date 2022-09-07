using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using ReportPortalNUnitLog4netClient.Core.Logging;
using ReportPortalNUnitLog4netClient.Models;
using ReportPortalNUnitLog4netClient.Models.Api;
using SoftAPIClient.Core;
using SoftAPIClient.RestSharpNewtonsoft;
using Attribute = ReportPortalNUnitLog4netClient.Models.Api.Attribute;

namespace ReportPortalNUnitLog4netClient.Core
{
    internal class ReportPortalEnabled : IReportPortalService
    {
        private readonly object _lockObj = new object();
        private const string ProductBug = "PB001";
        private readonly List<KeyValuePair<string, string>> _defectsResolutions = new List<KeyValuePair<string, string>>();

        private readonly IReportPortalClient _service;
        private readonly RpConfiguration _rpConfiguration;
        private static ConcurrentDictionary<string, Tuple<string, string, string, string>> _tests;
        private static ConcurrentDictionary<string, string> _suites;
        private static ConcurrentDictionary<string, string> _subSuites;
        private static ConcurrentDictionary<string, string> _parentTestItem;
        private static ConcurrentDictionary<string, int> _countOfTheTestsExecutions;
        private static readonly string RpDataFileName = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}rp_data.txt";

        private Launch Launch { get; set; }

        public ReportPortalEnabled(RpConfiguration rpConfiguration, List<KeyValuePair<string, string>> defectsMapping = null)
        {
            _rpConfiguration = rpConfiguration;
            RestClient.Instance.AddResponseConvertor(new RestSharpResponseConverter());

            _service = RestClient.Instance.GetService<IReportPortalClient>(new ReportPortalBaseInterceptor(rpConfiguration));
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
        public IReportPortalService StartLaunch(List<Attribute> tags = null)
        {
            var startTime = DateTime.UtcNow;
            var launch = _service.StartLaunch(new StartLaunchRequest
            {
                Name = _rpConfiguration.LaunchName,
                StartTime = startTime,
                Mode = _rpConfiguration.Mode,
                Attributes = tags
            }).Invoke().Body;
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
                var endTime = DateTime.UtcNow;
                Launch.EndTime = endTime;

                if (isForceFinish)
                {
                    _service.StopLaunch(Launch.Id, new FinishLaunchRequest
                    {
                        EndTime = endTime
                    }).Invoke();
                }
                else
                {
                    _service.FinishLaunch(Launch.Id, new FinishLaunchRequest
                    {
                        EndTime = endTime
                    }).Invoke();
                }

                if (_rpConfiguration.IsWriteLaunchData)
                {
                    WriteLaunchDataToFile();
                }
            }
            return this;
        }

        public IReportPortalService StartTest(TestContext.TestAdapter test, string suiteName, string subSuite, List<Attribute> tags, string testCodeId = null, List<string> tmsIds = null)
        {
            lock (_lockObj)
            {
                //check if suite is started
                if (!_suites.ContainsKey(suiteName))
                {
                    var suiteItem = _service.StartTestItem(new StartTestItemRequest
                    {
                        LaunchId = Launch.Id,
                        Name = suiteName,
                        StartTime = DateTime.UtcNow,
                        Attributes = new List<Attribute> { new Attribute { Key = "suite", Value = suiteName } },
                        Type = TestItemType.Suite
                    }).Invoke().Body;
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
                    var subSuiteItem = _service.StartTestItem(suiteId, new StartTestItemRequest
                    {
                        LaunchId = Launch.Id,
                        Name = subSuite,
                        StartTime = DateTime.UtcNow,
                        Attributes = new List<Attribute> { new Attribute { Key = "subSuite", Value = subSuite } },
                        Type = TestItemType.Suite,
                    }).Invoke().Body;
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
                    var parentItem = _service.StartTestItem(subSuiteId, new StartTestItemRequest
                    {
                        LaunchId = Launch.Id,
                        Name = GetParentItemName(test),
                        StartTime = DateTime.UtcNow,
                        Type = TestItemType.Test,
                        Attributes = tags,
                        Description = GetTestParentDescription(testCodeId, tmsIds)
                    }).Invoke().Body;
                    _parentTestItem.GetOrAdd(parentItemName, parentItem.Id);
                }
            }

            var testName = GetTestName(test);
            var parentId = _parentTestItem[parentItemName];
            var testItem = _service.StartTestItem(parentId, new StartTestItemRequest
            {
                LaunchId = Launch.Id,
                Name = testName,
                StartTime = DateTime.UtcNow,
                Type = TestItemType.Step,
                Description = GetTestDataDescription(test)
            }).Invoke().Body;

            var resultItem = new Tuple<string, string, string, string>(testItem.Id, parentId, _subSuites[suitePlusSubSuiteKey], _suites[suiteName]);
            _tests.GetOrAdd(GetTestId(test), resultItem);
            return this;
        }

        public IReportPortalService FinishTest(TestContext.TestAdapter test, TestStatus testStatus, string errorMessage, List<string> ticketIds)
        {
            var testId = GetTestId(test);
            if (!_tests.ContainsKey(testId))
            {
                return this;
            }
            var testItem = _tests[testId];

            var status = testStatus.ToRpStatus();
            if (status != Status.Passed)
            {
                Log(test, LogLevel.Error, DateTime.UtcNow, errorMessage);
            }

            var issue = GetIssue(status, ticketIds, out var isProductBug);
            _service.FinishTestItem(testItem.Item1, new FinishTestItemRequest
            {
                EndTime = DateTime.UtcNow,
                Status = status,
                Issue = issue,
                Description = BuildDescriptionForTheTestItem(ticketIds, issue, isProductBug)
            }).Invoke();

            return this;
        }

        public IReportPortalService Log(TestContext.TestAdapter test, LogLevel level, DateTime time, string text, Attach attach = null)
        {
            var testId = GetTestId(test);
            if (!_tests.ContainsKey(testId))
            {
                return this;
            }
            var testItem = _tests[testId];
            var request = new AddLogItemRequest
            {
                ItemId = testItem.Item1,
                Level = level,
                Time = time,
                Message = text
            };
            if (attach == null)
            {
                _service.AddLogItem(request).Invoke();
            }
            else
            {
                var fileParameter = new FileParameter("file", attach.Bytes, attach.Name, attach.ContentType);
                request.File = attach;
                var requestBody = new List<AddLogItemRequest>
                {
                    request
                };
                _service.AddLogItem(requestBody, fileParameter).Invoke();
            }
            return this;
        }

        public Launch GetLaunch()
        {
            return Launch;
        }

        public IReportPortalService CreateDashBoard(RpDashboard rpDashboard)
        {
            var filter = rpDashboard.Filter;
            filter.Id = _service.CreateFilter(filter).Invoke().Body.Id;
            var dashboard = rpDashboard.Dashboard;
            dashboard.Id = _service.CreateDashboard(rpDashboard.Dashboard).Invoke().Body.Id;

            rpDashboard.Widgets.ForEach(w =>
            {
                w.FilterIds = new List<long> { filter.Id };
                w.Filters = new List<FilterShort>
                {
                    new FilterShort
                    {
                        Name = filter.Name,
                        Value = filter.Id.ToString()
                    }
                };
                w.Id = _service.CreateWidget(w).Invoke().Body.Id;
                var addWidgetRequest = new AddWidgetRequest
                {
                    AddWidget = new AddWidget
                    {
                        WidgetId = w.Id,
                        WidgetName = w.Name,
                        WidgetType = w.WidgetType,
                        WidgetPosition = w.WidgetPosition,
                        WidgetSize = w.WidgetSize
                    }
                }; 
                _service.AddWidget(dashboard.Id, addWidgetRequest).Invoke();
            });
            return this;
        }

        private string GetTestId(TestContext.TestAdapter test) => test.ID;

        private string GetParentItemName(TestContext.TestAdapter testAdapter)
        {
            return testAdapter.MethodName;
        }

        private string GetTestName(TestContext.TestAdapter testAdapter)
        {
            var testArguments = testAdapter.Arguments;
            var testName = GetTestNameShort(testAdapter);
            if (testArguments?.Length == 0)
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
                _service.FinishTestItem(pair.Value, new FinishTestItemRequest
                {
                    EndTime = DateTime.UtcNow,
                }).Invoke();
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
                var resolutionResults = _defectsResolutions.FirstOrDefault(p => errorMessage.Contains(p.Key));
                if (!resolutionResults.Equals(default(KeyValuePair<string, string>)))
                {
                    isProductBug = false;
                    return new Issue
                    {
                        IssueType = resolutionResults.Value,
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
                IssueType = ProductBug,
                Comment = comment,
                AutoAnalyzed = false,
                IgnoreAnalyzer = true,
                ExternalSystemIssues = BuildExternalSystemIssues(ticketIds)
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

        private List<ExternalSystemIssue> BuildExternalSystemIssues(IEnumerable<string> ticketIds)
        {
            const string undefinedValue = "undefined_value";
            var btsProject = _rpConfiguration.IssueProjectName ?? undefinedValue;
            var btsUrl = _rpConfiguration.IssueUri ?? undefinedValue;
            var ticketUrl = _rpConfiguration.IssueTicketUrl ?? undefinedValue;
            return ticketIds.Select(ticketId => new ExternalSystemIssue
            {
                Url = ticketUrl + ticketId,
                BtsUrl = btsUrl,
                BtsProject = btsProject,
                TicketId = ticketId
            }).ToList();
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
            return testAdapter.Arguments?.Length == 0 ? null : $"InputData:{Environment.NewLine}{DecorateArguments(testAdapter.Arguments)}";
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
            if (arguments == null)
            {
                return string.Empty;
            }
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
