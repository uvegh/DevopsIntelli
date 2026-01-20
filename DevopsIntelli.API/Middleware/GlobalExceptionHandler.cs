using DevopsIntelli.Application.common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.WebRequestMethods;

namespace DevopsIntelli.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _environment;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public  async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        _logger.LogDebug("An unhandled exception occured. tracedId: {traceId}", traceId);

        var (statusCode, title, detail) = exception switch
        {
            ValidationException validationExc => (
             StatusCodes.Status400BadRequest,
             " Validaiton Error",
             $" bad request{string.Join(", ", validationExc.Errors.SelectMany(e => e.Value.Select(v => $"{e.Key}:  {v}")

             )


             )}"),
            NotFoundException notFoundExc => (
            StatusCodes.Status404NotFound,
            "Not found error",
            notFoundExc.Message),
            UnauthorizedAccessException unAuthorizedExc => (
            StatusCodes.Status401Unauthorized,
            "unauthorized user",
            $"You are not authorized to access this resource " +
            $" {unAuthorizedExc.Message}"
            ),
            _ => (
           StatusCodes.Status500InternalServerError,
           "Internal Server Error",
           _environment.IsDevelopment() ? exception.Message : "Something went wrong"

           )

          
           

        };
        // Create RFC 7807 Problem Details response
        // WHY: Standard format for HTTP API errors
        // INTERVIEW: "I follow RFC 7807 for consistent API error responses"

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Title = title,
            Type = $"http://httpstatus.com/{statusCode}",
            Extensions =
            {
                ["trace-id"]=traceId,
                ["timestamp"]= DateTime.UtcNow.ToString("0")

            }
        };

        //set the response br
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";
        //add problem details response
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;// exception handled
    }

    
}
