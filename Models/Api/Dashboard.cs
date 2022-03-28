using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Dashboard
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("share")]
        public bool Share { get; set; }
    }
}
