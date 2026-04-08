using PowerPlantApi.Exceptions;
using PowerPlantApi.Models;

namespace PowerPlantApi.Services;

/// <summary>
/// Optimized and deterministic production planner. Non-optimal.
///
/// Strategy:
/// 1. Compute costs (merit order)
/// 2. Use greedy
/// 3. Apply controlled adjustments
///
/// </summary>
public class ProductionPlanService
{
    private const double Co2EmissionFactor = 0.3;
    private const double Precision = 0.1;

    public List<PowerplantResult> ComputeProductionPlan(ProductionPlanRequest request)
    {
        double load = request.Load;
        var plants = Enrich(request);

        // Maintain insertion order (equivalent of LinkedHashMap)
        var production = new Dictionary<string, double>(
            plants.Select(p => new KeyValuePair<string, double>(p.Name, 0.0))
        );

        // Handle wind first (zero cost, always preferred)
        foreach (var p in plants)
        {
            if (p.Type == PowerplantType.Windturbine)
            {
                double prod = Round(Math.Min(p.Pmax, load));
                production[p.Name] = prod;
                load -= prod;
            }
        }

        if (load < -Precision)
            throw new ProductionPlanException("Wind exceeds load");

        // Sort dispatchable plants by cost (merit order)
        var dispatchable = plants
            .Where(p => p.Type != PowerplantType.Windturbine)
            .OrderBy(p => p.Cost)
            .ToList();

        var active = new List<Plant>();

        // Greedy dispatch
        foreach (var p in dispatchable)
        {
            if (load <= 0) break;
            if (p.Pmax <= 0) continue;

            double prod = Math.Min(p.Pmax, load);
            if (prod < p.Pmin) continue;

            prod = Round(Math.Max(prod, p.Pmin));

            production[p.Name] = prod;
            active.Add(p);
            load -= prod;
        }

        // Adjust upward when remaining load is too high
        if (load > Precision)
        {
            for (int i = active.Count - 1; i >= 0 && load > Precision; i--)
            {
                var p = active[i];
                double current  = production[p.Name];
                double headroom = p.Pmax - current;

                if (headroom <= 0) continue;

                double add = Round(Math.Min(headroom, load));
                production[p.Name] = Round(current + add);
                load -= add;
            }
        }

        // Adjust downward when output is too high
        if (load < -Precision)
        {
            for (int i = active.Count - 1; i >= 0 && load < -Precision; i--)
            {
                var p = active[i];
                double current   = production[p.Name];
                double reducible = current - p.Pmin;

                if (reducible <= 0) continue;

                double reduce = Round(Math.Min(reducible, -load));
                production[p.Name] = Round(current - reduce);
                load += reduce;
            }
        }

        if (Math.Abs(load) > Precision)
            throw new ProductionPlanException("Unable to match load precisely");

        return production
            .Select(kvp => new PowerplantResult(kvp.Key, Round(kvp.Value)))
            .ToList();
    }

    private static List<Plant> Enrich(ProductionPlanRequest request)
    {
        var result = new List<Plant>();

        foreach (var p in request.Powerplants)
        {
            double cost = 0;
            double pmax = p.Pmax;

            switch (p.Type)
            {
                case PowerplantType.Windturbine:
                    pmax = p.Pmax * request.Fuels.Wind / 100.0;
                    break;

                case PowerplantType.Gasfired:
                    cost = request.Fuels.Gas / p.Efficiency
                         + request.Fuels.Co2 * Co2EmissionFactor;
                    break;

                case PowerplantType.Turbojet:
                    cost = request.Fuels.Kerosine / p.Efficiency;
                    break;
            }

            result.Add(new Plant(p.Name, p.Type, p.Pmin, pmax, cost));
        }

        return result;
    }

    private static double Round(double v) => Math.Round(v * 10.0) / 10.0;

    /// <summary>
    /// Internal enriched plant record
    /// </summary>
    private record Plant(
        string Name,
        PowerplantType Type,
        double Pmin,
        double Pmax,
        double Cost
    );
}
