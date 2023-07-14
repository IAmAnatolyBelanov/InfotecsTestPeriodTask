using Mapster;
using Monitoring.Contracts.DeviceEvents;
using Monitoring.Dal.Models;

namespace Monitoring.Domain.Mappers;

/// <summary>
/// Маппер между <see cref="DeviceEvent"/> и <see cref="DeviceEventDtoLight"/>.
/// </summary>
[Mapper]
public interface IDeviceEventLightMapper : IBaseMapper<DeviceEvent, DeviceEventDtoLight>
{

}
