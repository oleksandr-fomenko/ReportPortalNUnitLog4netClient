using System.Collections.Generic;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using NUnit.Framework;
using ReportPortalNUnitLog4netClient.Models.Api;

namespace ReportPortalNUnitLog4netClient.Core.Logging
{
    public class ReportPortalLoggingStrategy : ILoggingStrategy
    {
        private const string RpConversionPatternInfo = "%message";
        private static readonly Level DefaultLogLevel = Level.All;
        public IAppender GetAppender()
        {
            var layout = new PatternLayout(RpConversionPatternInfo);
            layout.ActivateOptions();
            var appender = new CustomReportPortalAppender
            {
                Threshold = DefaultLogLevel,
                Layout = layout
            };
            appender.ActivateOptions();
            return appender;
        }

        public class CustomReportPortalAppender : AppenderSkeleton
        {
            protected Dictionary<Level, LogLevel> LevelMap = new Dictionary<Level, LogLevel>();

            public CustomReportPortalAppender()
            {
                LevelMap[Level.Fatal] = LogLevel.Fatal;
                LevelMap[Level.Error] = LogLevel.Error;
                LevelMap[Level.Warn] = LogLevel.Warning;
                LevelMap[Level.Info] = LogLevel.Info;
                LevelMap[Level.Debug] = LogLevel.Debug;
                LevelMap[Level.Trace] = LogLevel.Trace;
            }
            protected override void Append(LoggingEvent loggingEvent)
            {
                var logLevel = LogLevel.Info;
                if (LevelMap.ContainsKey(loggingEvent.Level))
                    logLevel = LevelMap[loggingEvent.Level];
                ReportPortalService.Instance.Log(TestContext.CurrentContext.Test, logLevel, loggingEvent.TimeStampUtc, RenderLoggingEvent(loggingEvent));
            }
        }
    }
}
