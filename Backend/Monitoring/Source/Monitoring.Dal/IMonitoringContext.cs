using Infotecs.Monitoring.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Monitoring.Dal;
/// <summary>
/// Контекст для работы с БД.
/// </summary>
public interface IMonitoringContext
{
    /// <summary>
    /// Коллекция всех сущностей <see cref="DeviceInfo"/> в контексте.
    /// </summary>
    DbSet<DeviceInfo> Devices { get; }

    /// <summary>
    /// Сохраняет изменения в контексте БД.
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Задача, представляющая асинхронную операцию сохранения. Результат задачи содержит количество записей состояния, записанных в БД.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
