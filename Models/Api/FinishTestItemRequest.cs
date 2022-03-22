using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class FinishTestItemRequest
    {
        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }
        [JsonProperty("issue")]
        public Issue Issue { get; set; }
        [JsonProperty("launchUuid")]
        public string LaunchUuid { get; set; }
        [JsonProperty("retry")]
        public bool Retry { get; set; }
        [JsonProperty("retryOf")]
        public string RetryOf { get; set; }
        [JsonProperty("status")]
        public Status? Status { get; set; }
        [JsonProperty("testCaseId")]
        public string TestCaseId { get; set; }
    }
}
