using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class StartLaunchRequest
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("mode")]
        public LaunchMode? Mode { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("rerun")]
        public bool Rerun { get; set; }
        [JsonProperty("rerunOf")]
        public string RerunOf { get; set; }
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
        [JsonIgnore]
        public string StartTimeString => ConvertTimeToMilliseconds(StartTime);

        protected string ConvertTimeToMilliseconds(DateTime date)
        {
            return ((long)date.Subtract(UnixEpoch).TotalMilliseconds).ToString();
        }
    }
}
