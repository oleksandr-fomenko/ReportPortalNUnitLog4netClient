﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    public class Widget
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("contentParameters")]
        public ContentParameters ContentParameters { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("filters")]
        public List<FilterShort> Filters { get; set; }
        [JsonProperty("filterIds")]
        public List<long> FilterIds { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("share")]
        public bool Share { get; set; }
        [JsonProperty("widgetType")]
        public string WidgetType { get; set; }
        [JsonIgnore]
        public WidgetPosition WidgetPosition { get; set; }
        [JsonIgnore]
        public WidgetSize WidgetSize { get; set; }
    }
}
