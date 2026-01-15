using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace MyApp.WebAPI.Middlewares;

/// <summary>
/// Global exception handler using .NET 8 IExceptionHandler
/// </summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, errorCode, message, errors) = exception switch
        {
            ValidationException validationException => (
                (int)HttpStatusCode.BadRequest,
                "VALIDATION_ERROR",
                validationException.Message,
                validationException.Errors as object
            ),
            NotFoundException notFoundException => (
                (int)HttpStatusCode.NotFound,
                "NOT_FOUND",
                notFoundException.Message,
                null
            ),
            AppException appException => (
                appException.StatusCode,
                appException.ErrorCode ?? "APP_ERROR",
                appException.Message,
                null
            ),
            _ => (
                (int)HttpStatusCode.InternalServerError,
                "INTERNAL_SERVER_ERROR",
                "An unexpected error occurred.",
                null
            )
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        // Create a response following the Result pattern or ProblemDetails
        // Since the user uses a Result class, let's look at it again to match the structure
        // I'll use a structure compatible with their Result class
        var response = new
        {
            IsSuccess = false,
            Message = message,
            Errors = errors is IDictionary<string, string[]> dict ? dict.Values.SelectMany(x => x).ToArray() :
                     (errors != null ? new[] { errors.ToString() } : Array.Empty<string>()),
            StatusCode = statusCode,
            ErrorCode = errorCode,
            TraceId = httpContext.TraceIdentifier
        };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
