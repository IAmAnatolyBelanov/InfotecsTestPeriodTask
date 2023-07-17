using Monitoring.Dal.Models;
using Monitoring.Dal.Sessions;

namespace Monitoring.Dal.Repositories;

/// <summary>
/// Репозиторий для работы с <see cref="DeviceEvent"/>.
/// </summary>
public interface IEventRepository
{
    /// <summary>
    /// Возвращает событие по запрошенному id.
    /// </summary>
    /// <param name="session"><see cref="ISession"/>.</param>
    /// <param name="id">Id события.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Событие по запрошенному id.</returns>
    /// <remarks>Если события по запрошенному id не существует, вернёт null.</remarks>
    Task<DeviceEvent?> GetEvent(ISession session, Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает список событий по запрошенному id девайса.
    /// </summary>
    /// <param name="session"><see cref="ISession"/>.</param>
    /// <param name="deviceId">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Список событий по запрошенному id девайса.</returns>
    /// <remarks>Если девайса не существует, список будет пустым.</remarks>
    Task<IReadOnlyList<DeviceEvent>> GetEventsByDevice(ISession session, Guid deviceId, CancellationToken cancellationToken);

    /// <summary>
    /// Добавляет события в БД.
    /// </summary>
    /// <param name="session"><see cref="ISession"/>.</param>
    /// <param name="deviceEvents">События.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task AddEvents(ISession session, IReadOnlyCollection<DeviceEvent> deviceEvents, CancellationToken cancellationToken);
}
