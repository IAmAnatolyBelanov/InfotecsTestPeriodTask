using Dapper;
using Monitoring.Dal.Models;
using Monitoring.Shared.Paginations;
using Monitoring.Dal.Sessions;

namespace Monitoring.Dal.Repositories;

/// <inheritdoc cref="IDeviceRepository"/>
public class DeviceRepository : IDeviceRepository
{
    private const string DbName = "public.devices";


    /// <inheritdoc/>
    public async Task InsertOrUpdateDevice(ISession session, DeviceInfo device, CancellationToken cancellationToken)
    {
        var parameters = new
        {
            device.Id,
            device.UserName,
            device.OperationSystemType,
            device.OperationSystemInfo,
            device.AppVersion,
        };

        const string Query = $@"
INSERT INTO {DbName}
    (id,
    user_name,
    operation_system_type,
    operation_system_info,
    app_version,
    last_update)
VALUES
    (@{nameof(parameters.Id)},
    @{nameof(parameters.UserName)},
    @{nameof(parameters.OperationSystemType)},
    @{nameof(parameters.OperationSystemInfo)},
    @{nameof(parameters.AppVersion)},
    NOW())
ON CONFLICT (id) DO UPDATE SET
    user_name = @{nameof(parameters.UserName)},
    operation_system_type = @{nameof(parameters.OperationSystemType)},
    operation_system_info = @{nameof(parameters.OperationSystemInfo)},
    app_version = @{nameof(parameters.AppVersion)},
    last_update = NOW();";

        await session.ExecuteAsync(Query, parameters, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<DeviceInfo>> GetDevices(ISession session, Pagination pagination, CancellationToken cancellationToken)
    {
        var parameters = new
        {
            Limit = pagination.PageSize,
            Offset = pagination.PageIndex * pagination.PageSize,
        };

        const string Query = $@"
SELECT
    id,
    user_name,
    operation_system_type,
    operation_system_info,
    app_version,
    last_update
FROM
    {DbName}
ORDER BY
    last_update DESC
LIMIT
    @{nameof(parameters.Limit)}
OFFSET
    @{nameof(parameters.Offset)};";

        var result = await session.QueryAsync<DeviceInfo>(Query, parameters, cancellationToken);
        return result.AsList();
    }

    /// <inheritdoc/>
    public async Task<DeviceInfo?> GetDevice(ISession session, Guid id, CancellationToken cancellationToken)
    {
        var parameters = new
        {
            Id = id,
        };

        const string Query = $@"
SELECT
    id,
    user_name,
    operation_system_type,
    operation_system_info,
    app_version,
    last_update
FROM
    {DbName}
WHERE
    id = @{nameof(parameters.Id)};";

        var devices = await session.QueryAsync<DeviceInfo>(Query, parameters, cancellationToken);

        var result = devices.FirstOrDefault();
        return result;
    }
}
