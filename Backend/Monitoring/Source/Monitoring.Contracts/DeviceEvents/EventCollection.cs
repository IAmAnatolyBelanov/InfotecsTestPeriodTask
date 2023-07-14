namespace Monitoring.Contracts.DeviceEvents;

/// <summary>
/// Список событий по конкретному девайсу.
/// </summary>
public class EventCollection
{
    /// <summary>
    /// Id девайса.
    /// </summary>
    public Guid DeviceId { get; set; }

    /// <summary>
    /// Список событий.
    /// </summary>
    public IReadOnlyList<DeviceEventDtoLight> Events { get; set; } = Array.Empty<DeviceEventDtoLight>();
}
