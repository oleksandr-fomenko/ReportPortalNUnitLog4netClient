using System;
using System.Collections.Generic;
using ReportPortalNUnitLog4netClient.Models;
using ReportPortalNUnitLog4netClient.Models.Api;
using RestSharp;
using SoftAPIClient.Attributes;
using SoftAPIClient.Core.Interfaces;
using SoftAPIClient.MetaData;

namespace ReportPortalNUnitLog4netClient.Core
{
    [Client]
    public interface IReportPortalClient
    {
        [RequestMapping(Method.POST, Path = "/{projectName}/launch", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<Launch>> StartLaunch([Body] StartLaunchRequest body);

        [RequestMapping(Method.PUT, Path = "/{projectName}/launch/{launchId}/stop", Headers = new[] { "Content-Type=application/json"})]
        Func<ResponseGeneric<Launch>> StopLaunch([PathParameter("launchId")] string launchId, [Body] FinishLaunchRequest body);

        [RequestMapping(Method.PUT, Path = "/{projectName}/launch/{launchId}/finish", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<Launch>> FinishLaunch([PathParameter("launchId")] string launchId, [Body] FinishLaunchRequest body);

        [RequestMapping(Method.POST, Path = "/{projectName}/item", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<TestItem>> StartTestItem([Body] StartTestItemRequest body);
        
        [RequestMapping(Method.POST, Path = "/{projectName}/item/{parentItem}", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<TestItem>> StartTestItem([PathParameter("parentItem")] string parentItem, [Body] StartTestItemRequest body);

        [RequestMapping(Method.PUT, Path = "/{projectName}/item/{testItemId}", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<TestItem>> FinishTestItem([PathParameter("testItemId")] string testItemId, [Body] FinishTestItemRequest body);

        [RequestMapping(Method.POST, Path = "/{projectName}/log", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<LogItem>> AddLogItem([Body] AddLogItemRequest body);

        [RequestMapping(Method.POST, Path = "/{projectName}/log", Headers = new[]{ "Content-Type=multipart/form-data" })]
        Func<ResponseGeneric<LogItem>> AddLogItem([Body(BodyType.Json, "json_request_part")] List<AddLogItemRequest> body, [File] SoftAPIClient.Core.FileParameter fileParameter);
    }

    public class ReportPortalBaseInterceptor : IRequestInterceptor
    {
        private Uri Uri { get; }
        private string Password { get; }
        private string Project { get; }
        private string ApiVersion { get; }

        public ReportPortalBaseInterceptor(RpConfiguration rpConfiguration)
        {
            Uri = rpConfiguration.Uri;
            Password = rpConfiguration.Uuid;
            Project = rpConfiguration.Project;
            ApiVersion = rpConfiguration.ApiVersion;
        }

        public Request Intercept()
        {
            var uri = Uri.ToString();
            if (!Uri.LocalPath.ToLowerInvariant().Contains($"api/{ApiVersion}"))
            {
                uri = $"{Uri}/api/{ApiVersion}";
            }
            return new Request
            {
                Url = uri,
                Headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("User-Agent", "SoftAPIClient"),
                    new KeyValuePair<string, string>("Authorization", "Bearer " + Password)
                },
                PathParameters = new Dictionary<string, object>
                {
                    { "projectName", Project }
                }
            };
        }
    }

}
