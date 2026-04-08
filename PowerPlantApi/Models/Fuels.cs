using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PowerPlantApi.Models;

public class Fuels
{
    [JsonPropertyName("gas(euro/MWh)")]
    [Range(0, double.MaxValue)]
    public double Gas { get; set; }

    [JsonPropertyName("kerosine(euro/MWh)")]
    [Range(0, double.MaxValue)]
    public double Kerosine { get; set; }

    [JsonPropertyName("co2(euro/ton)")]
    [Range(0, double.MaxValue)]
    public double Co2 { get; set; }

    [JsonPropertyName("wind(%)")]
    [Range(0, double.MaxValue)]
    public double Wind { get; set; }
}
