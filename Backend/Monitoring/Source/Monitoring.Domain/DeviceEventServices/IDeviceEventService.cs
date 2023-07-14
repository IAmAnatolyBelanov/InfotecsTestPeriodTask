using Monitoring.Dal.Models;

namespace Monitoring.Domain.DeviceEventServices;

/// <summary>
/// Бизнес-логика работы с событиями.
/// </summary>
public interface IDeviceEventService
{
    /// <summary>
    /// Добавляет событие в БД.
    /// </summary>
    /// <param name="deviceEvent">Событие.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Id добавленного события.</returns>
    Task<Guid> AddEvent(DeviceEvent deviceEvent, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает список событий по запрошенному девайсу.
    /// </summary>
    /// <param name="deviceId">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Список событий по запрошенному девайсу.</returns>
    /// <remarks>Если девайса не существует, список будет пустым.</remarks>
    Task<IReadOnlyList<DeviceEvent>> GetEventsByDevice(Guid deviceId, CancellationToken cancellationToken);
}
