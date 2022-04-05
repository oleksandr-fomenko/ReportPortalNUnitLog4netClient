using System;
using NUnit.Framework.Interfaces;
using ReportPortalNUnitLog4netClient.Models.Api;

namespace ReportPortalNUnitLog4netClient.Core.Logging
{
    public static class ReportPortalExtensions
    {
        public static Status ToRpStatus(this TestStatus testStatus)
        {
            Status status;
            switch (testStatus)
            {
                case TestStatus.Failed:
                    status = Status.Failed;
                    break;
                case TestStatus.Passed:
                    status = Status.Passed;
                    break;
                case TestStatus.Inconclusive:
                    status = Status.Interrupted;
                    break;
                case TestStatus.Skipped:
                    status = Status.Skipped;
                    break;
                case TestStatus.Warning:
                    status = Status.Skipped;
                    break;
                default:
                    throw new Exception($"Can't convert NUnit status {testStatus} to RP status");
            }
            return status;
        }

    }
}
