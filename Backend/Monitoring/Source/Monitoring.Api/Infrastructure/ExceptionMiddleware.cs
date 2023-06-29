using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infotecs.Monitoring.Shared.Exceptions;
using Infotecs;
using Infotecs.Monitoring;
using Infotecs.Monitoring.Api;

namespace Infotecs.Monitoring.Api.Infrastructure;

/// <summary>
/// Middleware для глобального отлова исключений.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Входная точка middleware.
    /// </summary>
    /// <param name="context">Контекст запроса.</param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ClientException ex)
        {
            _logger.LogError(ex, "Failed to execute request. Client exception.");

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var fail = new BaseResponse<object> { Error = ex.Message };

            await context.Response.WriteAsync(JsonSerializer.Serialize(fail));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute request. Unhandled exception.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

#if DEBUG
            var error = new { Info = "Unhandled exception", ex.Message, ex.StackTrace };
            var fail = new BaseResponse<object> { Error = error.ToString() };
#else
            var fail = new BaseResponse<object> { Error = "Unknown error" };
#endif

            await context.Response.WriteAsync(JsonSerializer.Serialize(fail));
        }
    }
}
