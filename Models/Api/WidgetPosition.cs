using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class WidgetPosition
    {
        [JsonProperty("positionX")]
        public long PositionX { get; set; }
        [JsonProperty("positionY")]
        public long PositionY { get; set; }
    }
}
