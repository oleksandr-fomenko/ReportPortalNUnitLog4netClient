using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ViewMode
    {
        Donut,
        Panel,
    }
}
