using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReportPortalNUnitLog4netClient.Models.Api
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TestItemType
    {
        None,
        [EnumMember(Value = "SUITE")] Suite,
        [EnumMember(Value = "TEST")] Test,
        [EnumMember(Value = "STEP")] Step,
        [EnumMember(Value = "BEFORE_CLASS")] BeforeClass,
        [EnumMember(Value = "AFTER_CLASS")] AfterClass,
        [EnumMember(Value = "AFTER_METHOD")] AfterMethod,
        [EnumMember(Value = "BEFORE_METHOD")] BeforeMethod,
    }
}
