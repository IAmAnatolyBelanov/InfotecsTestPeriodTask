using Monitoring.Contracts.DeviceInfo;
using Monitoring.Dal.Models;
using Mapster;

namespace Monitoring.Domain.Mappers;

/// <summary>
/// Маппер между <see cref="DeviceInfo"/> и <see cref="DeviceInfoDto"/>.
/// </summary>
[Mapper]
public interface IDeviceInfoMapper : IBaseMapper<DeviceInfo, DeviceInfoDto>
{

}
