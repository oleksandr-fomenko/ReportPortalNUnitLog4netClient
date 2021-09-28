using System.Collections.Generic;
using System.Linq;
using log4net.Config;

namespace ReportPortalNUnitLog4netClient.Core.Logging
{
    public static class CustomLogger
    {
        public static void Configure(bool isRpEnabled)
        {
            var appenders = new List<ILoggingStrategy>
            {
                new NUnitLoggingStrategy()
            };
            if (isRpEnabled)
            {
                appenders.Add(new ReportPortalLoggingStrategy());
            }
            BasicConfigurator.Configure(appenders.Select(a => a.GetAppender()).ToArray());
        }
    }
}
