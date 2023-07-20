namespace Monitoring.Contracts.Dtos.DeviceEvents;

/// <summary>
/// Облегчённая версия <see cref="DeviceEventDto"/>. Не содержит <see cref="DeviceEventDto.DeviceId"/>.
/// </summary>
public class DeviceEventLightDto
{
    /// <summary>
    /// Наименование события.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Дата события.
    /// </summary>
    public DateTimeOffset DateTime { get; set; }
}
