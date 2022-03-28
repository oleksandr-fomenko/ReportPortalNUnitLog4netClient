using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class AddWidget
    {
        [JsonProperty("widgetId")]
        public long WidgetId { get; set; }
        [JsonProperty("widgetName")]
        public string WidgetName { get; set; }
        [JsonProperty("widgetType")]
        public string WidgetType { get; set; }
        [JsonProperty("widgetPosition")]
        public WidgetPosition WidgetPosition { get; set; }
        [JsonProperty("widgetSize")]
        public WidgetSize WidgetSize { get; set; }
    }
}
