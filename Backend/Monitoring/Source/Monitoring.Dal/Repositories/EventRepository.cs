using Dapper;
using Monitoring.Dal.Models;
using Monitoring.Dal.Sessions;

namespace Monitoring.Dal.Repositories;

/// <inheritdoc cref="IEventRepository"/>.
public class EventRepository : IEventRepository
{
    private const string DbName = "public.events";

    /// <inheritdoc/>
    public async Task AddEvents(ISession session, IReadOnlyCollection<DeviceEvent> deviceEvents, CancellationToken cancellationToken)
    {
        const string Query = $@"
COPY {DbName}
    (id,
    device_id,
    name,
    date)
FROM STDIN (FORMAT BINARY);";

        await session.ImportAsync(Query, deviceEvents, (importer, value) =>
        {
            importer.Write(value.Id);
            importer.Write(value.DeviceId);
            importer.Write(value.Name);
            importer.Write(value.Date);
        }, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<DeviceEvent>> GetEventsByDevice(ISession session, Guid deviceId, CancellationToken cancellationToken)
    {
        var parameters = new
        {
            Id = deviceId,
        };

        const string Query = $@"
SELECT
    id,
    device_id,
    name,
    date
FROM
    {DbName}
WHERE
    device_id = @{nameof(parameters.Id)}
ORDER BY
    date DESC,
    id;";

        var result = await session.QueryAsync<DeviceEvent>(Query, parameters, cancellationToken);
        return result.AsList();
    }

    /// <inheritdoc/>
    public async Task<DeviceEvent?> GetEvent(ISession session, Guid id, CancellationToken cancellationToken)
    {
        var parameters = new { id };

        const string Query = $@"
SELECT
    id,
    device_id,
    name,
    date
FROM
    {DbName}
WHERE
    id = @{nameof(parameters.id)};";

        var result = await session.QueryAsync<DeviceEvent>(Query, parameters, cancellationToken);
        return result.FirstOrDefault();
    }
}
