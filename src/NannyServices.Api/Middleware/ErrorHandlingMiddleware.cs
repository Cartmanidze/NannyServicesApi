using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace NannyServices.Api.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteProblemDetails(context, ex);
        }
    }

    private static Task WriteProblemDetails(HttpContext context, Exception exception)
    {
        var (status, title, detail) = exception switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, "Bad Request", exception.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, "Bad Request", exception.Message),
            ValidationException validationEx => (HttpStatusCode.BadRequest, "Validation Failed", 
                string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage))),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error", "An unexpected error occurred.")
        };

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            Detail = detail,
            Type = GetProblemType(status)
        };

        var json = JsonSerializer.Serialize(problem);
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problem.Status.Value;
        return context.Response.WriteAsync(json);
    }

    private static string GetProblemType(HttpStatusCode status) => status switch
    {
        HttpStatusCode.BadRequest => "https://httpstatuses.com/400",
        HttpStatusCode.NotFound => "https://httpstatuses.com/404",
        _ => "https://httpstatuses.com/500"
    };
}
