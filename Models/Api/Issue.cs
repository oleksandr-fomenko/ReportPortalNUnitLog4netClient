using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Issue
    {
        [JsonProperty("autoAnalyzed")]
        public bool AutoAnalyzed { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("externalSystemIssues")]
        public List<ExternalSystemIssue> ExternalSystemIssues { get; set; }
        [JsonProperty("ignoreAnalyzer")]
        public bool IgnoreAnalyzer { get; set; }
        [JsonProperty("issueType")]
        public string IssueType { get; set; }
    }
}
