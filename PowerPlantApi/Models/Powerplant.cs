using System.ComponentModel.DataAnnotations;

namespace PowerPlantApi.Models;

public class Powerplant
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public PowerplantType Type { get; set; }

    [Range(0, double.MaxValue)]
    public double Efficiency { get; set; }

    [Range(0, double.MaxValue)]
    public double Pmin { get; set; }

    [Range(0, double.MaxValue)]
    public double Pmax { get; set; }

    // Computed fields (not part of JSON deserialization)
    public double EffectivePmax { get; set; }
    public double CostPerMwh { get; set; }

    public override string ToString() =>
        $"Powerplant{{Name='{Name}', Type={Type}, Pmin={Pmin:F1}, Pmax={Pmax:F1}, EffectivePmax={EffectivePmax:F1}, Cost={CostPerMwh:F4}}}";
}
