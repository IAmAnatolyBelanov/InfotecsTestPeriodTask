using Serilog.Events;

namespace Infotecs.Monitoring.Api.Infrastructure.Logger;

public class SerilogConfig : ISerilogConfig
{
    /// <summary>
    /// Позиция секции в файле конфигурации.
    /// </summary>
    public const string Position = "Serilog";

    /// <inheritdoc/>
    public string FilePath { get; set; } = default!;

    /// <inheritdoc/>
    public int MaxFileSizeInMB { get; set; }

    /// <inheritdoc/>
    public int MaxRetainedFileCount { get; set; }

    /// <inheritdoc/>
    public string RowTemplate { get; set; } = default!;

    /// <inheritdoc/>
    public string? SeqUrl { get; set; }

    /// <inheritdoc/>
    public LogEventLevel MinimumLevel { get; set; }
}
