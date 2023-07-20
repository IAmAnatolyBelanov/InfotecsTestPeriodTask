using Monitoring.Contracts.Dtos.DeviceEvents;
using Monitoring.Contracts.Dtos.DeviceInfo;

namespace Monitoring.Contracts.Queries.Device;

/// <summary>
/// Запрос на регистрацию девайса и его событий.
/// </summary>
public class RegisterDeviceQuery
{
    /// <summary>
    /// Девайс.
    /// </summary>
    public DeviceInfoDto Device { get; set; } = default!;

    /// <summary>
    /// События.
    /// </summary>
    public IReadOnlyList<DeviceEventLightDto>? Events { get; set; }
}
