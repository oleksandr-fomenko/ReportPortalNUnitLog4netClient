using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LogLevel
    {
        [EnumMember(Value = "TRACE")] Trace,
        [EnumMember(Value = "DEBUG")] Debug,
        [EnumMember(Value = "INFO")] Info,
        [EnumMember(Value = "WARN")] Warning,
        [EnumMember(Value = "ERROR")] Error,
        [EnumMember(Value = "FATAL")] Fatal,
    }
}
