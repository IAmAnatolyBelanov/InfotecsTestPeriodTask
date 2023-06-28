using Serilog.Events;
using Serilog;
using System.Reflection;

namespace Infotecs.Monitoring.Api.Infrastructure;

public static class LoggerRegister
{
    public static void RegisterLogger(WebApplicationBuilder builder)
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
            .CreateLogger();
        builder.Host.UseSerilog(logger);
    }
}
