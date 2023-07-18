using Mapster;
using Monitoring.Contracts.DeviceEvents;
using Monitoring.Dal.Models;

namespace Monitoring.Domain.Mappers;

/// <summary>
/// Маппер между <see cref="DeviceEvent"/> и <see cref="DeviceEventLightDto"/>.
/// </summary>
[Mapper]
public interface IDeviceEventLightMapper : IBaseMapper<DeviceEvent, DeviceEventLightDto>
{

}
