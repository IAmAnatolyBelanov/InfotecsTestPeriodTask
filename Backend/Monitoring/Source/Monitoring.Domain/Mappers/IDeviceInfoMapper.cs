using Monitoring.Contracts.DeviceInfo;
using Monitoring.Dal.Models;
using Mapster;

namespace Monitoring.Domain.Mappers;

/// <summary>
/// Маппер между <see cref="DeviceInfo"/> и <see cref="DeviceInfoDto"/>.
/// </summary>
[Mapper]
public interface IDeviceInfoMapper
{
    /// <summary>
    /// Маппит <see cref="DeviceInfo"/> к <see cref="DeviceInfoDto"/>.
    /// </summary>
    /// <param name="deviceInfo">Исходный объект, который нужно смаппить.</param>
    /// <returns>Новый объект, полученный в результате маппинга исходного.</returns>
    DeviceInfoDto MapToDto(DeviceInfo deviceInfo);

    /// <summary>
    /// Маппит <see cref="DeviceInfo"/> к <see cref="DeviceInfoDto"/>.
    /// </summary>
    /// <param name="deviceInfo">Исходный объект, который нужно смаппить.</param>
    /// <param name="deviceInfoDto">Объект, в который нужно произвести маппинг.</param>
    /// <returns>Если deviceInfoDto - null, то новый объект, полученный в результате маппинга исходного, иначе deviceInfoDto с изменениями в результате маппинга.</returns>
    DeviceInfoDto MapToDto(DeviceInfo deviceInfo, DeviceInfoDto deviceInfoDto);

    /// <summary>
    /// Маппит <see cref="DeviceInfoDto"/> к <see cref="DeviceInfo"/>.
    /// </summary>
    /// <param name="deviceInfoDto">Исходный объект, который нужно смаппить.</param>
    /// <returns>Новый объект, полученный в результате маппинга исходного.</returns>
    DeviceInfo MapFromDto(DeviceInfoDto deviceInfoDto);

    /// <summary>
    /// Маппит <see cref="DeviceInfoDto"/> к <see cref="DeviceInfo"/>.
    /// </summary>
    /// <param name="deviceInfoDto">Исходный объект, который нужно смаппить.</param>
    /// <param name="deviceInfo">Объект, в который нужно произвести маппинг.</param>
    /// <returns>Если deviceInfo - null, то новый объект, полученный в результате маппинга исходного, иначе deviceInfo с изменениями в результате маппинга.</returns>
    DeviceInfo MapFromDto(DeviceInfoDto deviceInfoDto, DeviceInfo deviceInfo);
}
