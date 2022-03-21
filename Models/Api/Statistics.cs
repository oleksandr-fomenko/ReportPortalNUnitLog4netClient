using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Statistics
    {
        [JsonProperty("executions")]
        public Executions Executions { get; set; }
        [JsonProperty("defects")]
        public Defects Defects { get; set; }
    }
}
