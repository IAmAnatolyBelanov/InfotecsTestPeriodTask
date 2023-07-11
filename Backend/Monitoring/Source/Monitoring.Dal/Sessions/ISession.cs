using Dapper;

namespace Monitoring.Dal.Sessions;

public interface ISession : IDisposable, IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task<int> ExecuteAsync(CommandDefinition commandDefinition);

    Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition commandDefinition);
}
