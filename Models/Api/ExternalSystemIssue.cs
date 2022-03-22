using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class ExternalSystemIssue
    {
        [JsonProperty("btsProject")]
        public string BtsProject { get; set; }
        [JsonProperty("btsUrl")]
        public string BtsUrl { get; set; }
        [JsonProperty("submitDate")]
        public long SubmitDate { get; set; }
        [JsonProperty("ticketId")]
        public string TicketId { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
