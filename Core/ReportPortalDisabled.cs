using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using ReportPortalNUnitLog4netClient.Models;
using ReportPortalNUnitLog4netClient.Models.Api;
using Attribute = ReportPortalNUnitLog4netClient.Models.Api.Attribute;

namespace ReportPortalNUnitLog4netClient.Core
{
    internal class ReportPortalDisabled : IReportPortalService
    {
        public IReportPortalService StartLaunch(List<Attribute> tags = null)
        {
            return this;
        }

        public IReportPortalService FinishLaunch()
        {
            return this;
        }

        public IReportPortalService StartTest(TestContext.TestAdapter test, string suiteName, string subSuite, List<Attribute> tags, string testCodeId = null, List<string> tmsIds = null, List<Attribute> testItemTags = null)
        {
            return this;
        }

        public IReportPortalService FinishTest(TestContext.TestAdapter test, TestStatus status, string errorMessage, List<string> ticketIds)
        {
            return this;
        }

        public IReportPortalService Log(TestContext.TestAdapter test, LogLevel level, DateTime time, string text, Attach attach = null)
        {
            return this;
        }

        public Launch GetLaunch()
        {
            return new Launch();
        }

        public IReportPortalService CreateDashBoard(RpDashboard rpDashboard)
        {
            return this;
        }

        public string GetLaunchUiLink()
        {
            return string.Empty;
        }
    }
}
