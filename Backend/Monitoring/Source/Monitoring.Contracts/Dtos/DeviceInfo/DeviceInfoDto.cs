using Monitoring.Shared.OperationSystem;

namespace Monitoring.Contracts.Dtos.DeviceInfo;

/// <summary>
/// Dto информации о девайсе.
/// </summary>
public class DeviceInfoDto
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string UserName { get; set; } = default!;

    /// <summary>
    /// Тип ОС.
    /// </summary>
    public OperationSystemType OperationSystemType { get; set; }

    /// <summary>
    /// Подробное описание ОС.
    /// </summary>
    public string OperationSystemInfo { get; set; } = default!;

    /// <summary>
    /// Версия приложения.
    /// </summary>
    public string AppVersion { get; set; } = default!;

    /// <summary>
    /// Дата и время последнего обновления информации о девайсе.
    /// </summary>
    public DateTimeOffset LastUpdate { get; set; }
}
