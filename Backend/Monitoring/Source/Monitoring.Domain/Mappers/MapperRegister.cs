using Mapster;
using Monitoring.Contracts.Dtos.DeviceEvents;
using Monitoring.Contracts.Dtos.DeviceInfo;
using Monitoring.Dal.Models;

namespace Monitoring.Domain.Mappers;

/// <summary>
/// Регистратор мапперов. Необходим для работы автогенерации кода Mapster'а.
/// </summary>
public class MapperRegister : IRegister
{
    /// <summary>
    /// Конфигурирование мапперов, создаваемых в ходе автогенерации Mapster'а.
    /// </summary>
    /// <param name="config">Конфигурация.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DeviceInfoDto, DeviceInfo>()
            .Map(dst => dst.LastUpdate, src => src.LastUpdate.ToUniversalTime());

        config.NewConfig<DeviceEventDto, DeviceEvent>()
            .Map(dst => dst.DateTime, src => src.DateTime.ToUniversalTime());

        config.NewConfig<DeviceEventLightDto, DeviceEvent>()
            .Map(dst => dst.DateTime, src => src.DateTime.ToUniversalTime());
    }
}
