using Infotecs.Monitoring.Dal.Models;
using Infotecs.Monitoring.Shared.Paginations;
using Infotecs.Monitoring.Dal.Sessions;

namespace Infotecs.Monitoring.Dal.Repositories;

/// <summary>
/// Репозиторий для работы с DeviceInfo в БД PostgreSQL.
/// </summary>
public interface IPgDeviceRepository
{
    /// <summary>
    /// Возвращает DeviceInfo с запрошенным Id.
    /// </summary>
    /// <param name="session">Сессия PostgreSQL.</param>
    /// <param name="id">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>DeviceInfo с запрошенным Id.</returns>
    /// <remarks>Если девайса с указанным Id не найдено, вернёт null.</remarks>
    Task<DeviceInfo?> GetDevice(IPgSession session, Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает коллекцию девайсов.
    /// </summary>
    /// <param name="session">Сессия PostgreSQL.</param>
    /// <param name="pagination">Пагинация.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Коллекция девайсов.</returns>
    Task<IReadOnlyList<DeviceInfo>> GetDevices(IPgSession session, Pagination pagination, CancellationToken cancellationToken);

    /// <summary>
    /// Добавляет новый девайс либо обновляет существующий.
    /// </summary>
    /// <param name="session">Сессия PostgreSQL.</param>
    /// <param name="device">Девайс.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task MergeDevice(IPgSession session, DeviceInfo device, CancellationToken cancellationToken);
}
