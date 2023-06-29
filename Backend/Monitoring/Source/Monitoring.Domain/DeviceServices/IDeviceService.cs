using Infotecs.Monitoring.Dal.Models;
using Infotecs.Monitoring.Shared.Paginations;

namespace Infotecs.Monitoring.Domain.DeviceBizRules;

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
    ValueTask<IReadOnlyList<DeviceInfo>> GetAll(Pagination pagination, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает статистику по запрошенному девайсу.
    /// </summary>
    /// <param name="deviceId">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Статистика по запрошенному девайсу.</returns>
    ValueTask<DeviceStatistics> GetStatistics(Guid deviceId, CancellationToken cancellationToken);

    /// <summary>
    /// Добавляет или обновляет информацию о девайсе.
    /// </summary>
    /// <param name="device">Информация о девайсе.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns><see cref="ValueTask"/>.</returns>
    ValueTask AddOrUpdateDevice(DeviceInfo device, CancellationToken cancellationToken);
}
