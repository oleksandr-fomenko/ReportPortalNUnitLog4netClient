using System.Collections.Generic;
using ReportPortalNUnitLog4netClient.Models.Api;

namespace ReportPortalNUnitLog4netClient.Factory
{
    public static class RpWidgetFactory
    {
        public static Widget OverallStatistics(ViewMode viewMode = ViewMode.Donut)
        {
            return new Widget
            {
                Name = "Overall_Statistics",
                Description = "Overall statistics by filter",
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
                    {"viewMode", viewMode},
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
                    Width = 5
                }
            };
        }
    }
}
