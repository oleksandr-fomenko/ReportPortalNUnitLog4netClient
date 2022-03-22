using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class StartTestItemRequest
    {
        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("launch_id")]
        public string LaunchId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonProperty("type")]
        public TestItemType? Type { get; set; }
        [JsonProperty("uniqueId")]
        public string UniqueId { get; set; }
    }
}
