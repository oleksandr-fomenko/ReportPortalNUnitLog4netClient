using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Filter
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }
        [JsonProperty("share")]
        public bool Share { get; set; } = true;
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("conditions")]
        public List<Condition> Conditions { get; set; }
        [JsonProperty("orders")]
        public List<Order> Orders { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
