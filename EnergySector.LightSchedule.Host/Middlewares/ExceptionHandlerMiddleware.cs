using EnergySector.LightSchedule.Domain.Shared;
using System.Net;
using System.Text.Json;

namespace EnergySector.LightSchedule.Host.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (BusinessException buException)
        {
            _logger.LogError($"A business exception occurred: {buException}");
            await HandleBusinessExceptionAsync(httpContext, buException);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error." +
            "The service will be unavailable until the end of maintenance work"
        }));
    }

    private async Task HandleBusinessExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        }));
    }
}
