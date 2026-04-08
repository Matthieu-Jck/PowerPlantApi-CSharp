using System.ComponentModel.DataAnnotations;

namespace PowerPlantApi.Models;

public class ProductionPlanRequest
{
    [Range(0.001, double.MaxValue, ErrorMessage = "Load must be positive.")]
    public double Load { get; set; }

    [Required]
    public Fuels Fuels { get; set; } = null!;

    [Required]
    [MinLength(1, ErrorMessage = "At least one powerplant is required.")]
    public List<Powerplant> Powerplants { get; set; } = null!;
}
