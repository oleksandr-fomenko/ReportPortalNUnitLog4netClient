using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Condition
    {
        [JsonProperty("filteringField")]
        public string FilteringField { get; set; }
        [JsonProperty("condition")]
        public string ConditionType { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
