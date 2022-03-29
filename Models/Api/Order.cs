using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Order
    {
        [JsonProperty("sortingColumn")]
        public string SortingColumn { get; set; }
        [JsonProperty("isAsc")]
        public bool IsAsc { get; set; }
    }
}
