using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class AddWidgetRequest
    {
        [JsonProperty("addWidget")]
        public AddWidget AddWidget { get; set; }
    }
}
