using Dapper;
using Npgsql;

namespace Monitoring.Dal.Sessions;

public class PgSession : IPgSession
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction? _transaction;

    internal PgSession(string connectionString, bool beginTransaction = false)
    {
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
        if (beginTransaction)
        {
            _transaction = _connection.BeginTransaction();
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
        {
            throw new NullReferenceException("Transaction is not begun. Need to begin transaction before commiting.");
        }

        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task<int> ExecuteAsync(CommandDefinition commandDefinition)
    {
        return await _connection.ExecuteAsync(commandDefinition);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition commandDefinition)
    {
        return await _connection.QueryAsync<T>(commandDefinition);
    }

    public void Dispose()
    {
        if (_transaction != null)
        {
            _transaction.Dispose();
        }
        _connection.Close();
        _connection.Dispose();
    }

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
