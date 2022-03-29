using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class ContentParameters
    {
        [JsonProperty("contentFields")]
        public IEnumerable<string> ContentFields { get; set; }
        [JsonProperty("itemsCount")]
        public long ItemsCount { get; set; }
        [JsonProperty("widgetOptions")]
        public Dictionary<string, object> WidgetOptions { get; set; }
    }
}
