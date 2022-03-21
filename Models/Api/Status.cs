using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        [EnumMember(Value = "IN_PROGRESS")] InProgress,
        [EnumMember(Value = "PASSED")] Passed,
        [EnumMember(Value = "FAILED")] Failed,
        [EnumMember(Value = "SKIPPED")] Skipped,
        [EnumMember(Value = "INTERRUPTED")] Interrupted,
    }
}
