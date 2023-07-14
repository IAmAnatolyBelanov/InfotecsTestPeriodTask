namespace Monitoring.Contracts.DeviceEvents;

/// <summary>
/// Dto класса DeviceEvent.
/// </summary>
public class DeviceEventDto
{
    /// <summary>
    /// Id девайса.
    /// </summary>
    public Guid DeviceId { get; set; }

    /// <summary>
    /// Наименование события.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Дата события.
    /// </summary>
    public DateTimeOffset Date { get; set; }
}
