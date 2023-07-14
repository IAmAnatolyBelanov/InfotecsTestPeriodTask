namespace Monitoring.Contracts.DeviceEvents;

/// <summary>
/// Облегчённая версия <see cref="DeviceEventDto"/>. Не содержит <see cref="DeviceEventDto.DeviceId"/>.
/// </summary>
public class DeviceEventDtoLight
{
    /// <summary>
    /// Наименование события.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Дата события.
    /// </summary>
    public DateTimeOffset Date { get; set; }
}
