using Dapper;
using Npgsql;

namespace Monitoring.Dal.Sessions;

/// <summary>
/// Сессия для работы с БД.
/// </summary>
public interface ISession : IDisposable, IAsyncDisposable
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
    /// <param name="commandText">SQL запрос.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Количество изменённых строк.</returns>
    Task<int> ExecuteAsync(string commandText, CancellationToken cancellationToken);

    /// <summary>
    /// Исполняет команду.
    /// </summary>
    /// <param name="commandText">SQL запрос.</param>
    /// <param name="parameters">Параметры запроса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Количество изменённых строк.</returns>
    Task<int> ExecuteAsync(string commandText, object parameters, CancellationToken cancellationToken);

    /// <summary>
    /// Импортирует коллекцию объектов.
    /// </summary>
    /// <typeparam name="T">Тип импортируемых объектов.</typeparam>
    /// <param name="commandText">SQL запрос.</param>
    /// <param name="values">Импортируемые объекты.</param>
    /// <param name="map">Функция-маппер для бинарного импортирования объектов.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task ImportAsync<T>(string commandText, IReadOnlyCollection<T> values, Action<NpgsqlBinaryImporter, T> map, CancellationToken cancellationToken);

    /// <summary>
    /// Исполняет команду.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемых объектов.</typeparam>
    /// <param name="commandDefinition">Команда для выполнения.</param>
    /// <returns>Коллекция объектов типа T, полученных из БД в соответствии с переданной командой.</returns>
    Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition commandDefinition);

    /// <summary>
    /// Исполняет команду.
    /// </summary>
    /// <param name="commandText">SQL запрос.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Коллекция объектов типа T, полученных из БД в соответствии с переданной командой.</returns>
    Task<IEnumerable<T>> QueryAsync<T>(string commandText, CancellationToken cancellationToken);

    /// <summary>
    /// Исполняет команду.
    /// </summary>
    /// <param name="commandText">SQL запрос.</param>
    /// <param name="parameters">Парметры запроса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Коллекция объектов типа T, полученных из БД в соответствии с переданной командой.</returns>
    Task<IEnumerable<T>> QueryAsync<T>(string commandText, object parameters, CancellationToken cancellationToken);
}
