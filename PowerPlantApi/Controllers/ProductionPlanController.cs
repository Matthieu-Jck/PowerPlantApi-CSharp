using Microsoft.AspNetCore.Mvc;
using PowerPlantApi.Models;
using PowerPlantApi.Services;

namespace PowerPlantApi.Controllers;

/// <summary>
/// Equivalent of ProductionPlanController.java (@RestController).
/// POST /productionplan
/// </summary>
[ApiController]
[Route("productionplan")]
public class ProductionPlanController : ControllerBase
{
    private readonly ILogger<ProductionPlanController> _logger;
    private readonly ProductionPlanService _productionPlanService;

    public ProductionPlanController(
        ILogger<ProductionPlanController> logger,
        ProductionPlanService productionPlanService)
    {
        _logger = logger;
        _productionPlanService = productionPlanService;
    }

    /// <summary>
    /// POST /productionplan
    ///
    /// Accepts a payload describing the load, fuel prices and available powerplants,
    /// and returns the optimal production plan.
    /// </summary>
    [HttpPost]
    public ActionResult<List<PowerplantResult>> ComputeProductionPlan(
        [FromBody] ProductionPlanRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation(
            "Received production plan request: load={Load}MWh, plants={Count}",
            request.Load,
            request.Powerplants.Count);

        var result = _productionPlanService.ComputeProductionPlan(request);

        _logger.LogInformation(
            "Production plan computed successfully with {Count} entries",
            result.Count);

        return Ok(result);
    }
}
