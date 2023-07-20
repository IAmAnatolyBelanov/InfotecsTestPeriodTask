using Mapster;
using Monitoring.Contracts.Dtos.DeviceInfo;
using Monitoring.Dal.Models;

namespace Monitoring.Domain.Mappers;

/// <summary>
/// Маппер между <see cref="DeviceInfo"/> и <see cref="DeviceInfoDto"/>.
/// </summary>
[Mapper]
public interface IDeviceInfoMapper : IBaseMapper<DeviceInfo, DeviceInfoDto>
{

}
