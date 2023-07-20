namespace Monitoring.Contracts.Dtos.DeviceEvents;

/// <summary>
/// Dto для класса событий девайса.
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
    public DateTimeOffset DateTime { get; set; }
}
