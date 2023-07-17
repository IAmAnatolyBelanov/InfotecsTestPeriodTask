using Dapper;
using Npgsql;

namespace Monitoring.Dal.Sessions;

/// <inheritdoc cref="ISession"/>
public class Session : ISession
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction? _transaction;

    /// <summary>
    /// Конструктор класса <see cref="Session"/>.
    /// </summary>
    /// <param name="connectionString">Строка подключения к БД PostgreSQL.</param>
    /// <param name="beginTransaction">Необходимо ли открывать транзакцию.</param>
    internal Session(string connectionString, bool beginTransaction = false)
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
    public Task<int> ExecuteAsync(string commandText, CancellationToken cancellationToken)
    {
        return ExecuteAsync(new CommandDefinition(commandText: commandText, cancellationToken: cancellationToken));
    }

    /// <inheritdoc/>
    public Task<int> ExecuteAsync(string commandText, object parameters, CancellationToken cancellationToken)
    {
        return ExecuteAsync(new CommandDefinition(commandText: commandText, parameters: parameters, cancellationToken: cancellationToken));
    }

    /// <inheritdoc/>
    public async Task<int> ExecuteAsync(CommandDefinition commandDefinition)
    {
        return await _connection.ExecuteAsync(commandDefinition);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<T>> QueryAsync<T>(string commandText, CancellationToken cancellationToken)
    {
        return QueryAsync<T>(new CommandDefinition(commandText: commandText, cancellationToken: cancellationToken));
    }

    /// <inheritdoc/>
    public Task<IEnumerable<T>> QueryAsync<T>(string commandText, object parameters, CancellationToken cancellationToken)
    {
        return QueryAsync<T>(new CommandDefinition(commandText: commandText, parameters: parameters, cancellationToken: cancellationToken));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition commandDefinition)
    {
        return await _connection.QueryAsync<T>(commandDefinition);
    }

    /// <inheritdoc/>
    public async Task ImportAsync<T>(string commandText, IReadOnlyCollection<T> values, Action<NpgsqlBinaryImporter, T> map, CancellationToken cancellationToken)
    {
        await using (var importer = await _connection.BeginBinaryImportAsync(commandText, cancellationToken))
        {
            foreach (var value in values)
            {
                await importer.StartRowAsync(cancellationToken);
                map(importer, value);
            }

            await importer.CompleteAsync(cancellationToken);
        }
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
