using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class TestItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("parent")]
        public string ParentId { get; set; }
        [JsonProperty("uniqueId")]
        public string UniqueId { get; set; }
        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }
        [JsonProperty("codeRef")]
        public string CodeRef { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("hasStats")]
        public bool HasStats { get; set; }
        [JsonProperty("launchUuid")]
        public string LaunchUuid { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("retry")]
        public bool Retry { get; set; }
        [JsonProperty("retryOf")]
        public string RetryOf { get; set; }
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty("endTime")]
        public DateTime? EndTime { get; set; }
        [JsonProperty("testCaseId")]
        public string TestCaseId { get; set; }
        [JsonProperty("type")]
        public TestItemType? Type { get; set; }
    }
}
