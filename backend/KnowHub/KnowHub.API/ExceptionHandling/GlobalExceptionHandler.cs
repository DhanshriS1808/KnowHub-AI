using KnowHub.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace KnowHub.API.ExceptionHandling;

/// <summary>
/// Turns AppException subclasses into their intended status code and lets
/// everything else surface as a 500 without leaking exception detail.
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
        var appException = exception as AppException;

        var statusCode = appException?.StatusCode ?? StatusCodes.Status500InternalServerError;

        if (appException is not null)
        {
            _logger.LogInformation("Handled {Type}: {Message}", exception.GetType().Name, exception.Message);
        }
        else
        {
            _logger.LogError(exception, "Unhandled exception on {Path}", httpContext.Request.Path);
        }

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = ReasonFor(statusCode),
            // Only our own exception messages are safe to echo back to the caller.
            Detail = appException?.Message ?? "An unexpected error occurred.",
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private static string ReasonFor(int statusCode) => statusCode switch
    {
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status409Conflict => "Conflict",
        StatusCodes.Status500InternalServerError => "Internal Server Error",
        _ => "Request Error"
    };
}
