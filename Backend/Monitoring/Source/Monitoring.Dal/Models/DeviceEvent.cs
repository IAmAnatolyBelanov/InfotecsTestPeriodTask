namespace Monitoring.Dal.Models;

/// <summary>
/// Событие.
/// </summary>
public class DeviceEvent
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id девайса.
    /// </summary>
    public Guid DeviceId { get; set; }

    /// <summary>
    /// Наименование события.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Дата.
    /// </summary>
    public DateTimeOffset DateTime { get; set; }
}
