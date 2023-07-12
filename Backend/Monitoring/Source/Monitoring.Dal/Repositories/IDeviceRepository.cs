using Monitoring.Dal.Models;
using Monitoring.Shared.Paginations;
using Monitoring.Dal.Sessions;

namespace Monitoring.Dal.Repositories;

/// <summary>
/// Репозиторий для работы с <see cref="DeviceInfo"/>.
/// </summary>
public interface IDeviceRepository
{
    /// <summary>
    /// Возвращает DeviceInfo с запрошенным Id.
    /// </summary>
    /// <param name="session">Сессия PostgreSQL.</param>
    /// <param name="id">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>DeviceInfo с запрошенным Id.</returns>
    /// <remarks>Если девайса с указанным Id не найдено, вернёт null.</remarks>
    Task<DeviceInfo?> GetDevice(ISession session, Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает коллекцию девайсов.
    /// </summary>
    /// <param name="session">Сессия PostgreSQL.</param>
    /// <param name="pagination">Пагинация.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Коллекция девайсов.</returns>
    Task<IReadOnlyList<DeviceInfo>> GetDevices(ISession session, Pagination pagination, CancellationToken cancellationToken);

    /// <summary>
    /// Добавляет новый девайс либо обновляет существующий.
    /// </summary>
    /// <param name="session">Сессия PostgreSQL.</param>
    /// <param name="device">Девайс.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task InsertOrUpdateDevice(ISession session, DeviceInfo device, CancellationToken cancellationToken);
}
