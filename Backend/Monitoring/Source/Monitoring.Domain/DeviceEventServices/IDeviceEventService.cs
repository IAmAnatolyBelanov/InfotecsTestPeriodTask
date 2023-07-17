using Monitoring.Dal.Models;

namespace Monitoring.Domain.DeviceEventServices;

/// <summary>
/// Бизнес-логика работы с событиями.
/// </summary>
public interface IDeviceEventService
{
    /// <summary>
    /// Добавляет события.
    /// </summary>
    /// <param name="deviceEvents">События.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task AddEvents(IReadOnlyCollection<DeviceEvent> deviceEvents, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает список событий по запрошенному девайсу.
    /// </summary>
    /// <param name="deviceId">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Список событий по запрошенному девайсу.</returns>
    /// <remarks>Если девайса не существует, список будет пустым.</remarks>
    Task<IReadOnlyList<DeviceEvent>> GetEventsByDevice(Guid deviceId, CancellationToken cancellationToken);
}
