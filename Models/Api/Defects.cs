using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Defects
    {
        [JsonProperty("product_bug")]
        public Defect ProductBugs { get; set; }
        [JsonProperty("automation_bug")]
        public Defect AutomationBugs { get; set; }
        [JsonProperty("system_issue")]
        public Defect SystemIssues { get; set; }
        [JsonProperty("to_investigate")]
        public Defect ToInvestigate { get; set; }
        [JsonProperty("no_defect")]
        public Defect NoDefect { get; set; }
    }
}
