using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Executions
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("passed")]
        public int Passed { get; set; }
        [JsonProperty("failed")]
        public int Failed { get; set; }
        [JsonProperty("skipped")]
        public int Skipped { get; set; }
    }
}
