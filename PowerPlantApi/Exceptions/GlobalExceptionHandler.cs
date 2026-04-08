using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PowerPlantApi.Exceptions;

namespace PowerPlantApi.Exceptions;

/// <summary>
/// Global exception handler middleware — equivalent of GlobalExceptionHandler.java (@RestControllerAdvice).
/// Registered in Program.cs via app.UseExceptionHandler().
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int statusCode;
        string error;
        string message;
        Dictionary<string, string>? details = null;

        switch (exception)
        {
            case ProductionPlanException ppe:
                _logger.LogWarning("Business error: {Message}", ppe.Message);
                statusCode = StatusCodes.Status422UnprocessableEntity;
                error = "Unprocessable Entity";
                message = ppe.Message;
                break;

            case BadHttpRequestException bhre:
                _logger.LogWarning("Bad request: {Message}", bhre.Message);
                statusCode = StatusCodes.Status400BadRequest;
                error = "Bad Request";
                message = "Bad JSON request body";
                break;

            default:
                _logger.LogError(exception, "Unexpected error");
                statusCode = StatusCodes.Status500InternalServerError;
                error = "Internal Server Error";
                message = "Internal server error";
                break;
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var body = new Dictionary<string, object>
        {
            ["timestamp"] = DateTime.UtcNow.ToString("o"),
            ["status"]    = statusCode,
            ["error"]     = error,
            ["message"]   = message,
            ["path"]      = httpContext.Request.Path.ToString()
        };

        if (details is not null)
            body["details"] = details;

        await httpContext.Response.WriteAsJsonAsync(body, cancellationToken);
        return true;
    }
}
