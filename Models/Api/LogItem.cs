using System;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class LogItem
    {
        [JsonProperty("binaryContent")]
        public BinaryContent BinaryContent { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("itemId")]
        public string ItemId { get; set; }
        [JsonProperty("launchId")]
        public string LaunchId { get; set; }
        [JsonProperty("level")]
        public LogLevel? Level { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
        [JsonProperty("time")]
        public DateTime Time { get; set; }
        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }
}
