using Mapster;
using Monitoring.Contracts.DeviceEvents;
using Monitoring.Contracts.DeviceInfo;
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
        config.NewConfig<DeviceInfo, DeviceInfoDto>();

        config.NewConfig<DeviceEventDto, DeviceEvent>()
            .Map(dst => dst.Date, src => src.Date.ToUniversalTime());

        config.NewConfig<DeviceEventDtoLight, DeviceEvent>()
            .Map(dst => dst.Date, src => src.Date.ToUniversalTime());
    }
}
