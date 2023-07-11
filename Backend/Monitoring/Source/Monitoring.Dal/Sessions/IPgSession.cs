using Dapper;

namespace Infotecs.Monitoring.Dal.Sessions;

/// <summary>
/// Сессия для работы с БД PostgreSQL.
/// </summary>
public interface IPgSession : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Коммитит изменения в БД в пределах открытой транзакции.
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task CommitAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Исполняет команду.
    /// </summary>
    /// <param name="commandDefinition">Команда для выполнения.</param>
    /// <returns>Количество изменённых строк.</returns>
    Task<int> ExecuteAsync(CommandDefinition commandDefinition);

    /// <summary>
    /// Исполняет команду.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемых объектов.</typeparam>
    /// <param name="commandDefinition">Команда для выполнения.</param>
    /// <returns>Коллекция объектов типа T, полученных из БД в соответствии с переданной командой.</returns>
    Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition commandDefinition);
}
