using Mapster;
using Monitoring.Contracts.Dtos.DeviceEvents;
using Monitoring.Dal.Models;

namespace Monitoring.Domain.Mappers;

/// <summary>
/// Маппер между <see cref="DeviceEvent"/> и <see cref="DeviceEventDto"/>.
/// </summary>
[Mapper]
public interface IDeviceEventMapper : IBaseMapper<DeviceEvent, DeviceEventDto>
{

}
