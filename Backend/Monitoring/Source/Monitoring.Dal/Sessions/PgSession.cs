using Dapper;
using Npgsql;

namespace Infotecs.Monitoring.Dal.Sessions;

/// <inheritdoc cref="IPgSession"/>
public class PgSession : IPgSession
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction? _transaction;

    /// <summary>
    /// Конструктор класса <see cref="PgSession"/>.
    /// </summary>
    /// <param name="connectionString">Строка подключения к БД PostgreSQL.</param>
    /// <param name="beginTransaction">Необходимо ли открывать транзакцию.</param>
    internal PgSession(string connectionString, bool beginTransaction = false)
    {
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
        if (beginTransaction)
        {
            _transaction = _connection.BeginTransaction();
        }
    }

    /// <inheritdoc/>
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
        {
            throw new NullReferenceException("Transaction is not begun. Need to begin transaction before commiting.");
        }

        await _transaction.CommitAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> ExecuteAsync(CommandDefinition commandDefinition)
    {
        return await _connection.ExecuteAsync(commandDefinition);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition commandDefinition)
    {
        return await _connection.QueryAsync<T>(commandDefinition);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_transaction != null)
        {
            _transaction.Dispose();
        }
        _connection.Close();
        _connection.Dispose();
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
        await _connection.CloseAsync();
        await _connection.DisposeAsync();
    }
}
