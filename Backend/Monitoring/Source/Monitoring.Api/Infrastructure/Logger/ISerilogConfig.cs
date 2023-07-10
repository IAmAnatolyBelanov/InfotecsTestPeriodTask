using Serilog.Events;

namespace Infotecs.Monitoring.Api.Infrastructure.Logger;

public interface ISerilogConfig
{
    /// <summary>
    /// Путь до файла логов.
    /// </summary>
    string FilePath { get; }

    /// <summary>
    /// Максимальный размер файла логов.
    /// </summary>
    int MaxFileSizeInMB { get; }

    /// <summary>
    /// Максимальное число файлов лога, которые будут храниться, включая текущий файл.
    /// </summary>
    int MaxRetainedFileCount { get; }

    /// <summary>
    /// Шаблон строки лога.
    /// </summary>
    string RowTemplate { get; }

    /// <summary>
    /// Url сервиса Seq.
    /// </summary>
    string? SeqUrl { get; }

    /// <summary>
    /// Минимальный уровень логов.
    /// </summary>
    LogEventLevel MinimumLevel { get; }
}
