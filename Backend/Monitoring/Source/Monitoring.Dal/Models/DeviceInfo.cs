namespace Infotecs.Monitoring.Dal.Models;

/// <summary>
/// Информация о девайсе.
/// </summary>
public class DeviceInfo
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
    /// Дата и время последнего обновления информации о девайсе.
    /// </summary>
    public DateTimeOffset LastUpdate { get; set; }
}
