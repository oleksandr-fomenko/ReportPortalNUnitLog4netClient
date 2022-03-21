using System;
using NUnit.Framework.Interfaces;
using ReportPortalNUnitLog4netClient.Models.Api;

namespace ReportPortalNUnitLog4netClient.Core.Logging
{
    public static class ReportPortalExtensions
    {
        public static Status ToRpStatus(this TestStatus testStatus)
        {
            return testStatus switch
            {
                TestStatus.Failed => Status.Failed,
                TestStatus.Passed => Status.Passed,
                TestStatus.Inconclusive => Status.Interrupted,
                TestStatus.Skipped => Status.Skipped,
                TestStatus.Warning => Status.Skipped,
                _ => throw new Exception($"Can't convert NUnit status {testStatus} to RP status"),
            };
        }

    }
}
