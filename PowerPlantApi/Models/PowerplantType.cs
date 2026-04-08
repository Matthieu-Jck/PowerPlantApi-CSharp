using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PowerPlantApi.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PowerplantType
{
    [EnumMember(Value = "gasfired")]
    Gasfired,

    [EnumMember(Value = "turbojet")]
    Turbojet,

    [EnumMember(Value = "windturbine")]
    Windturbine
}
