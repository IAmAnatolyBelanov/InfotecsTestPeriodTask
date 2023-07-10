using Serilog;

namespace Infotecs.Monitoring.Api.Infrastructure.Logger;

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
        var config = RegisterConfig(builder);

        RegisterLogger(builder, config);
    }

    /// <summary>
    /// Регистрирует конфигурацию Serilog.
    /// </summary>
    /// <param name="builder">Builder сервиса, куда необходимо зарегистрировать конфигурацию.</param>
    /// <returns>Заполненная конфигурация.</returns>
    /// <remarks>Так как конфиг логгера нужен уже на этапе регистрации логгера, то приходится его создавать вручную.</remarks>
    private static ISerilogConfig RegisterConfig(WebApplicationBuilder builder)
    {
        var config = new SerilogConfig();

        builder.Configuration.Bind(SerilogConfig.Position, config);
        builder.Services.AddSingleton<ISerilogConfig>(config);

        return config;
    }

    private static void RegisterLogger(WebApplicationBuilder builder, ISerilogConfig config)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Is(config.MinimumLevel);

        loggerConfiguration = loggerConfiguration
            .WriteTo.File(
                path: config.FilePath,
                restrictedToMinimumLevel: config.MinimumLevel,
                fileSizeLimitBytes: config.MaxFileSizeInMB * 1024,
                rollOnFileSizeLimit: true,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: config.MaxRetainedFileCount,
                outputTemplate: config.RowTemplate
            );

        if (!string.IsNullOrWhiteSpace(config.SeqUrl))
        {
            loggerConfiguration = loggerConfiguration
                .WriteTo.Seq(config.SeqUrl);
        }

        var logger = loggerConfiguration.CreateLogger();

        builder.Host.UseSerilog(logger);
    }
}
