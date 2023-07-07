using System.Reflection;
using Serilog;
using Serilog.Events;

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
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.File(
                path: $"C:\\Logs\\{Assembly.GetExecutingAssembly().GetName().Name}\\log_.log",
                restrictedToMinimumLevel: LogEventLevel.Debug,
                fileSizeLimitBytes: 1024 * 1024 * 100,
                rollOnFileSizeLimit: true,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();
        builder.Host.UseSerilog(logger);
    }
}
