using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Defect
    {
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
