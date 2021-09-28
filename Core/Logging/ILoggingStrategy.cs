using log4net.Appender;

namespace ReportPortalNUnitLog4netClient.Core.Logging
{
    public interface ILoggingStrategy
    {
        IAppender GetAppender();
    }
}
