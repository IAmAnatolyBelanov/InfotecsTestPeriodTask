namespace Monitoring.Contracts.Dtos.DeviceEvents;

/// <summary>
/// Список событий по конкретному девайсу.
/// </summary>
public class EventCollectionDto
{
    /// <summary>
    /// Id девайса.
    /// </summary>
    public Guid DeviceId { get; set; }

    /// <summary>
    /// Список событий.
    /// </summary>
    public IReadOnlyList<DeviceEventLightDto> Events { get; set; } = Array.Empty<DeviceEventLightDto>();
}
