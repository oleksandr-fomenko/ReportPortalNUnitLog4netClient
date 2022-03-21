using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Attribute
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("system")]
        public bool System { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
