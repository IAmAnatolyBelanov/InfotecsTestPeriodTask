using Infotecs.Monitoring.Contracts.DeviceInfo;
using Infotecs.Monitoring.Dal.Models;
using Mapster;

namespace Infotecs.Monitoring.Domain.Mappers;

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
    }
}
