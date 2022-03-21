using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class BinaryContent
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("thumbnailId")]
        public string ThumbnailId { get; set; }
    }
}
