using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using ReportPortal.Client.Models;

namespace ReportPortalNUnitLog4netClient.Core
{
    public interface IReportPortalService
    {
        IReportPortalService StartLaunch(List<string> tags = null);
        IReportPortalService FinishLaunch();
        IReportPortalService StartTest(TestContext.TestAdapter test, string suiteName, string subSuite, List<string> tags, string testCodeId = null, List<string> tmsIds = null);
        IReportPortalService FinishTest(TestContext.TestAdapter test, TestStatus status, string errorMessage, List<string> ticketIds);
        IReportPortalService Log(TestContext.TestAdapter test, LogLevel level, DateTime time, string text, Attach attach = null);
        Launch GetLaunch();
    }
}
