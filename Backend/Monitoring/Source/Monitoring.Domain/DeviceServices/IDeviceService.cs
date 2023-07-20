using Monitoring.Dal.Models;
using Monitoring.Shared.Paginations;

namespace Monitoring.Domain.DeviceServices;

/// <summary>
/// Бизнес-логика работы с девайсами.
/// </summary>
public interface IDeviceService
{
    /// <summary>
    /// Возвращает информацию о девайсах.
    /// </summary>
    /// <param name="pagination">Пагинация.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Массив данных о девайсах.</returns>
    Task<IReadOnlyList<DeviceInfo>> GetAll(Pagination pagination, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает статистику по запрошенному девайсу.
    /// </summary>
    /// <param name="deviceId">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Статистика по запрошенному девайсу.</returns>
    Task<DeviceStatistics> GetStatistics(Guid deviceId, CancellationToken cancellationToken);

    /// <summary>
    /// Добавляет или обновляет информацию о девайсе. Если переданы события, добавляет их тоже.
    /// </summary>
    /// <param name="device">Информация о девайсе.</param>
    /// <param name="events">События.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task AddOrUpdateDevice(DeviceInfo device, IReadOnlyList<DeviceEvent>? events, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает <see cref="DeviceInfo"/> с запрошенным Id.
    /// </summary>
    /// <param name="id">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns><see cref="DeviceInfo"/> с запрошенным Id.</returns>
    /// <remarks>Если девайса с указанным Id не найдено, вернёт null.</remarks>
    Task<DeviceInfo?> Get(Guid id, CancellationToken cancellationToken);
}
