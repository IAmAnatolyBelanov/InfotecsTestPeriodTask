namespace Monitoring.Domain.DeviceServices;

/// <summary>
/// Статистика по девайсу.
/// </summary>
public class DeviceStatistics
{
    /// <summary>
    /// Id девайса.
    /// </summary>
    public Guid DeviceId { get; set; }

    /// <summary>
    /// Дата последнего изменения информации о девайсе.
    /// </summary>
    public DateTimeOffset? LastUpdate { get; set; }
}
