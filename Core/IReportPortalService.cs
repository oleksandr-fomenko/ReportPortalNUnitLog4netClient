using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using ReportPortalNUnitLog4netClient.Models;
using ReportPortalNUnitLog4netClient.Models.Api;
using Attribute = ReportPortalNUnitLog4netClient.Models.Api.Attribute;

namespace ReportPortalNUnitLog4netClient.Core
{
    public interface IReportPortalService
    {
        IReportPortalService StartLaunch(List<Attribute> tags = null);
        IReportPortalService FinishLaunch();
        IReportPortalService StartTest(TestContext.TestAdapter test, string suiteName, string subSuite, List<Attribute> tags, string testCodeId = null, List<string> tmsIds = null);
        IReportPortalService FinishTest(TestContext.TestAdapter test, TestStatus status, string errorMessage, List<string> ticketIds);
        IReportPortalService Log(TestContext.TestAdapter test, LogLevel level, DateTime time, string text, Attach attach = null);
        Launch GetLaunch();
        IReportPortalService CreateDashBoard(RpDashboard rpDashboard);
    }
}
