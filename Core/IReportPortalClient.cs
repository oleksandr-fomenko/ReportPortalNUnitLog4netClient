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
    [Client(ResponseInterceptors = new[] { typeof(ReportPortalResponseInterceptor) })]
    public interface IReportPortalClient
    {
        [RequestMapping(Method.GET, Path = "/{projectName}/launch/uuid/{launchUuid}", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<Launch>> GetLaunchByUuid([PathParameter("launchUuid")] string launchUuId);

        [RequestMapping(Method.GET, Path = "/{projectName}/launch/{launchId}", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<Launch>> GetLaunchById([PathParameter("launchId")] string launchUuId);

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

        [RequestMapping(Method.POST, Path = "/{projectName}/filter", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<Filter>> CreateFilter([Body] Filter body);

        [RequestMapping(Method.POST, Path = "/{projectName}/dashboard", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<Dashboard>> CreateDashboard([Body] Dashboard body);

        [RequestMapping(Method.POST, Path = "/{projectName}/widget", Headers = new[] { "Content-Type=application/json" })]
        Func<ResponseGeneric<Widget>> CreateWidget([Body] Widget body);

        [RequestMapping(Method.PUT, Path = "/{projectName}/dashboard/{dashboardId}/add", Headers = new[] { "Content-Type=application/json" })]
        Func<Response> AddWidget([PathParameter("dashboardId")] long dashboardId, [Body] AddWidgetRequest body);
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
                uri = $"{Uri}api/{ApiVersion}";
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

    public class ReportPortalResponseInterceptor : IResponseInterceptor
    {
        public void ProcessResponse(Response response)
        {
            if ((int)response.HttpStatusCode >= 400)
            {
                throw new Exception($"Report Portal API throws exception: HttpStatusCode - {response.HttpStatusCode}, Body - {response.ResponseBodyString}");
            }
            if (response.Exception != null)
            {
                throw response.Exception;
            }
        }
    }
}
