using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LaunchMode
    {
        [EnumMember(Value = "DEFAULT")]
        Default,
        [EnumMember(Value = "DEBUG")]
        Debug
    }
}
