using Serilog;

namespace Infotecs.Monitoring.Api.Infrastructure;

/// <summary>
/// Класс для регистрации логгера в сервисе.
/// </summary>
public static class LoggerRegister
{
    /// <summary>
    /// Регистрирует логгер.
    /// </summary>
    /// <param name="builder">Builder сервиса, куда необходимо зарегистрировать логгер.</param>
    public static void RegisterLogger(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
    }
}
