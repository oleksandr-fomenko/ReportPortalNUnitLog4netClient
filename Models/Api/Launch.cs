using System;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Launch : StartLaunchRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }
        [JsonIgnore]
        public string EndTimeString => ConvertTimeToMilliseconds(EndTime);
        [JsonProperty("number")]
        public long Number { get; set; }
        [JsonProperty("owner")]
        public string Owner { get; set; }
        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
