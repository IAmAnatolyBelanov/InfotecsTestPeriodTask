using System.Net;
using System.Text.Json;
using Monitoring.Shared.Exceptions;

namespace Monitoring.Api.Infrastructure;

/// <summary>
/// Middleware для глобального отлова исключений.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    /// <summary>
    /// Конструктор класса <see cref="ExceptionMiddleware"/>.
    /// </summary>
    /// <param name="next">Следующий делегат запроса.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="webHostEnvironment">Переменные окружения.</param>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment webHostEnvironment)
    {
        _next = next;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// Входная точка middleware.
    /// </summary>
    /// <param name="context">Контекст запроса.</param>
    /// <returns><see cref="Task"/>.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ServerException ex)
        {
            await CatchClientException(context, ex);
        }
        catch (Exception ex)
        {
            await CatchUnhandledException(context, ex);
        }
    }

    private async Task CatchClientException(HttpContext context, ServerException ex)
    {
        _logger.LogError(ex, "Failed to execute request. Client exception.");

        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        var fail = new BaseResponse<object> { Error = ex.Message };

        await context.Response.WriteAsync(JsonSerializer.Serialize(fail));
    }

    private async Task CatchUnhandledException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Failed to execute request. Unhandled exception.");

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        BaseResponse<object> fail;

        if (_webHostEnvironment.IsDevelopment())
        {
            var error = new { Info = "Unhandled exception", ex.Message, ex.StackTrace };
            fail = new BaseResponse<object> { Error = error.ToString() };
        }
        else
        {
            fail = new BaseResponse<object> { Error = "Unknown error" };
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(fail));
    }
}
