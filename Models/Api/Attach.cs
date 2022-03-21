using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Attach
    {
        [JsonProperty("name")]
        public string Name { get; }
        [JsonIgnore]
        public byte[] Bytes { get; }
        [JsonIgnore]
        public string ContentType { get; }

        public Attach(string name,
            byte[] bytes,
            string contentType)
        {
            Name = name;
            ContentType = contentType;
            Bytes = bytes;
        }
    }
}
