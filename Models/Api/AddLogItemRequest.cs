using System;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class AddLogItemRequest
    {
        [JsonProperty("item_id")]
        public string ItemId { get; set; }
        [JsonProperty("level")]
        public LogLevel? Level { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("time")]
        public DateTime Time { get; set; }
        [JsonProperty("file")]
        public Attach File { get; set; }
    }
}
