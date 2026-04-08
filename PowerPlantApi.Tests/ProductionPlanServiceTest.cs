using PowerPlantApi.Exceptions;
using PowerPlantApi.Models;
using PowerPlantApi.Services;
using Xunit;

namespace PowerPlantApi.Tests;

/// <summary>
/// Equivalent of ProductionPlanServiceTest.java (JUnit 5 → xUnit).
/// @Test → [Fact]
/// assertEquals → Assert.Equal
/// assertThrows → Assert.Throws
/// assertTrue  → Assert.True
/// </summary>
public class ProductionPlanServiceTest
{
    private readonly ProductionPlanService _service = new();

    // ---------- Helpers ----------

    private static Fuels BuildFuels(double wind) => new()
    {
        Gas      = 13.4,
        Kerosine = 50.8,
        Co2      = 20.0,
        Wind     = wind
    };

    private static Powerplant Plant(
        string name, PowerplantType type,
        double efficiency, double pmin, double pmax) => new()
    {
        Name       = name,
        Type       = type,
        Efficiency = efficiency,
        Pmin       = pmin,
        Pmax       = pmax
    };

    private static ProductionPlanRequest Request(
        double load, double wind, List<Powerplant> plants) => new()
    {
        Load        = load,
        Fuels       = BuildFuels(wind),
        Powerplants = plants
    };

    // ---------- Tests ----------

    [Fact]
    public void ShouldMatchLoadExactly()
    {
        var req = Request(
            200, 0,
            [Plant("gas1", PowerplantType.Gasfired, 0.5, 50, 200)]
        );

        var result = _service.ComputeProductionPlan(req);
        double total = result.Sum(r => r.P);

        Assert.Equal(200.0, total, precision: 1);
    }

    [Fact]
    public void ShouldUseWindFirst()
    {
        var req = Request(
            100, 100,
            [
                Plant("gas1",  PowerplantType.Gasfired,     0.5, 50,  200),
                Plant("wind1", PowerplantType.Windturbine,  1.0,  0,  150)
            ]
        );

        var result = _service.ComputeProductionPlan(req);
        var wind = result.First(r => r.Name == "wind1");

        Assert.Equal(100.0, wind.P, precision: 1);
    }

    [Fact]
    public void ShouldRespectPmin()
    {
        var req = Request(
            60, 0,
            [Plant("gas1", PowerplantType.Gasfired, 0.5, 50, 100)]
        );

        var result = _service.ComputeProductionPlan(req);
        double output = result[0].P;

        Assert.True(output == 0 || output >= 50);
    }

    [Fact]
    public void ShouldNotExceedPmax()
    {
        var req = Request(
            500, 0,
            [
                Plant("gas1", PowerplantType.Gasfired, 0.5, 50, 100),
                Plant("gas2", PowerplantType.Gasfired, 0.5, 50, 200)
            ]
        );

        var result = _service.ComputeProductionPlan(req);

        foreach (var r in result)
        {
            var original = req.Powerplants.First(p => p.Name == r.Name);
            Assert.True(r.P <= original.Pmax + 0.1);
        }
    }

    [Fact]
    public void ShouldThrowIfLoadTooHigh()
    {
        var req = Request(
            1000, 0,
            [Plant("small", PowerplantType.Gasfired, 0.5, 50, 100)]
        );

        Assert.Throws<ProductionPlanException>(
            () => _service.ComputeProductionPlan(req));
    }

    [Fact]
    public void ShouldHandleZeroWind()
    {
        var req = Request(
            200, 0,
            [
                Plant("wind1", PowerplantType.Windturbine, 1.0,  0, 150),
                Plant("gas1",  PowerplantType.Gasfired,    0.5, 50, 200)
            ]
        );

        var result = _service.ComputeProductionPlan(req);
        var wind = result.First(r => r.Name == "wind1");

        Assert.Equal(0.0, wind.P, precision: 1);
    }
}
