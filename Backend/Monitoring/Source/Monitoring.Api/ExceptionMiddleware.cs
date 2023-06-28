using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Monitoring.Shared.Exceptions;

namespace Monitoring.Api;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ClientException ex)
        {
            _logger.LogError(ex, "Failed to execute request. Client exception");

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
            var error = new { Info = "Unhandled exception", Message = ex.Message, StackTrace = ex.StackTrace };
            var fail = new BaseResponse<object> { Error = error.ToString() };
#else
            var fail = new BaseResponse<object> { Error = "Unknown error" };
#endif

            await context.Response.WriteAsync(JsonSerializer.Serialize(fail));
        }
    }
}
