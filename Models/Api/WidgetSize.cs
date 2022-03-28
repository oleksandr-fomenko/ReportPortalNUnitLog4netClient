using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class WidgetSize
    {
        [JsonProperty("width")]
        public long Width { get; set; }
        [JsonProperty("height")]
        public long Height { get; set; }
    }
}
