using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class FinishLaunchRequest
    {
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }
        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }
    }
}
