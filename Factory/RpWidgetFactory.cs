using System;
using System.Collections.Generic;
using ReportPortalNUnitLog4netClient.Models.Api;

namespace ReportPortalNUnitLog4netClient.Factory
{
    public static class RpWidgetFactory
    {
        private static readonly string UniquePostfix = $"{DateTime.Now:hhmmssff}";
        public static Widget LastLaunchStatistics()
        {
            return new Widget
            {
                Name = $"LAST LAUNCH STATISTICS_{UniquePostfix}",
                Description = "Last launch statistics",
                WidgetType = "overallStatistics",
                ContentParameters = new ContentParameters {
                ContentFields = new []{
                    "statistics$executions$total",
                    "statistics$executions$passed",
                    "statistics$executions$failed",
                    "statistics$executions$skipped",
                    "statistics$defects$product_bug$pb001",
                    "statistics$defects$automation_bug$ab001",
                    "statistics$defects$system_issue$si001",
                    "statistics$defects$to_investigate$ti001"
                },
                ItemsCount = 1,
                WidgetOptions = new Dictionary<string, object> {
                    {"viewMode", "donut"},
                    {"latest", true},
                }
                },
                Share = true,
                WidgetPosition = new WidgetPosition
                {
                    PositionX = 0,
                    PositionY = 0
                },
                WidgetSize = new WidgetSize
                {
                    Height = 5,
                    Width = 6
                }
            };
        }

        public static Widget LaunchResultTrend()
        {
            return new Widget
            {
                Name = $"LAUNCH RESULT TREND_{UniquePostfix}",
                Description = "Launch results trend",
                WidgetType = "statisticTrend",
                ContentParameters = new ContentParameters
                {
                    ContentFields = new[]{
                        "statistics$executions$passed",
                        "statistics$executions$failed",
                        "statistics$executions$skipped"
                    },
                    ItemsCount = 50,
                    WidgetOptions = new Dictionary<string, object> {
                        {"viewMode", "area-spline"},
                        {"timeline", "launch"},
                        {"zoom", false},
                    }
                },
                Share = true,
                WidgetPosition = new WidgetPosition
                {
                    PositionX = 6,
                    PositionY = 0
                },
                WidgetSize = new WidgetSize
                {
                    Height = 5,
                    Width = 6
                }
            };
        }

        public static Widget TestCasesTrend()
        {
            return new Widget
            {
                Name = $"TEST CASES GROWTH TREND CHART_{UniquePostfix}",
                Description = "Test cases trend",
                WidgetType = "casesTrend",
                ContentParameters = new ContentParameters
                {
                    ContentFields = new[]{
                        "statistics$executions$total"
                    },
                    ItemsCount = 50,
                    WidgetOptions = new Dictionary<string, object> {
                        {"timeline", "launch"}
                    }
                },
                Share = true,
                WidgetPosition = new WidgetPosition
                {
                    PositionX = 0,
                    PositionY = 12
                },
                WidgetSize = new WidgetSize
                {
                    Height = 5,
                    Width = 6
                }
            };
        }

        public static Widget LaunchesDurationChart()
        {
            return new Widget
            {
                Name = $"LAUNCHES DURATION CHART_{UniquePostfix}",
                Description = "Launches duration chart",
                WidgetType = "launchesDurationChart",
                ContentParameters = new ContentParameters
                {
                    ContentFields = new string[0],
                    ItemsCount = 50,
                    WidgetOptions = new Dictionary<string, object> {
                        {"timeline", "launch"}
                    }
                },
                Share = true,
                WidgetPosition = new WidgetPosition
                {
                    PositionX = 6,
                    PositionY = 12
                },
                WidgetSize = new WidgetSize
                {
                    Height = 5,
                    Width = 6
                }
            };
        }

        public static Widget FailedCasesTrend()
        {
            return new Widget
            {
                Name = $"FAILED CASES TREND CHART_{UniquePostfix}",
                Description = "Failed cases trend",
                WidgetType = "bugTrend",
                ContentParameters = new ContentParameters
                {
                    ContentFields = new[]{
                        "statistics$executions$failed"
                    },
                    ItemsCount = 50,
                    WidgetOptions = new Dictionary<string, object> {
                        {"timeline", "launch"}
                    }
                },
                Share = true,
                WidgetPosition = new WidgetPosition
                {
                    PositionX = 0,
                    PositionY = 17
                },
                WidgetSize = new WidgetSize
                {
                    Height = 5,
                    Width = 6
                }
            };
        }

        public static Widget CumulativeTrend(string key1, string key2)
        {
            return new Widget
            {
                Name = $"CUMULATIVE TREND_{UniquePostfix}",
                Description = "Cumulative trend",
                WidgetType = "cumulative",
                ContentParameters = new ContentParameters
                {
                    ContentFields = new[]{
                        "statistics$executions$failed",
                        "statistics$executions$skipped",
                        "statistics$executions$passed",
                        "statistics$defects$product_bug$total",
                        "statistics$defects$automation_bug$total",
                        "statistics$defects$system_issue$total",
                        "statistics$defects$no_defect$total",
                        "statistics$defects$to_investigate$total"
                    },
                    ItemsCount = 50,
                    WidgetOptions = new Dictionary<string, object> {
                        {"state", "ready"},
                        {"viewName", "widget"},
                        {"attributes", new []{ key1, key2}},
                        {"lastRefresh", new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()},
                    }
                },
                Share = true,
                WidgetPosition = new WidgetPosition
                {
                    PositionX = 0,
                    PositionY = 5
                },
                WidgetSize = new WidgetSize
                {
                    Height = 7,
                    Width = 12
                }
            };
        }

    }
}
