using Dapper;
using Infotecs.Monitoring.Dal.Models;
using Infotecs.Monitoring.Shared.Paginations;
using Monitoring.Dal.Sessions;

namespace Monitoring.Dal.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private const string _dbName = "public.devices";

    public async Task MergeDevice(IPgSession session, DeviceInfo device, CancellationToken cancellationToken)
    {
        var parameters = new
        {
            device.Id,
            device.UserName,
            device.OperationSystemType,
            device.OperationSystemInfo,
            device.AppVersion,
        };

        const string query = $@"
MERGE INTO {_dbName} AS TARGET
USING (SELECT @{nameof(parameters.Id)} AS {nameof(parameters.Id)}) AS SOURCE
ON SOURCE.{nameof(parameters.Id)} = TARGET.""{nameof(DeviceInfo.Id)}""
WHEN MATCHED THEN
UPDATE SET
    ""{nameof(DeviceInfo.UserName)}"" = @{nameof(parameters.UserName)},
    ""{nameof(DeviceInfo.OperationSystemType)}"" = @{nameof(parameters.OperationSystemType)},
    ""{nameof(DeviceInfo.OperationSystemInfo)}"" = @{nameof(parameters.OperationSystemInfo)},
    ""{nameof(DeviceInfo.AppVersion)}"" = @{nameof(parameters.AppVersion)},
    ""{nameof(DeviceInfo.LastUpdate)}"" = NOW()
WHEN NOT MATCHED THEN
INSERT
    (""{nameof(DeviceInfo.Id)}"",
    ""{nameof(DeviceInfo.UserName)}"",
    ""{nameof(DeviceInfo.OperationSystemType)}"",
    ""{nameof(DeviceInfo.OperationSystemInfo)}"",
    ""{nameof(DeviceInfo.AppVersion)}"",
    ""{nameof(DeviceInfo.LastUpdate)}"")
VALUES
    (@{nameof(parameters.Id)},
    @{nameof(parameters.UserName)},
    @{nameof(parameters.OperationSystemType)},
    @{nameof(parameters.OperationSystemInfo)},
    @{nameof(parameters.AppVersion)},
    NOW());";

        var command = new CommandDefinition(commandText: query, parameters: parameters, cancellationToken: cancellationToken);

        await session.ExecuteAsync(command);
    }

    public async Task<IReadOnlyList<DeviceInfo>> GetDevices(IPgSession session, Pagination pagination, CancellationToken cancellationToken)
    {
        var parameters = new
        {
            Limit = pagination.PageSize,
            Offset = pagination.PageIndex * pagination.PageSize,
        };

        const string query = $@"
SELECT
    ""{nameof(DeviceInfo.Id)}"",
    ""{nameof(DeviceInfo.UserName)}"",
    ""{nameof(DeviceInfo.OperationSystemType)}"",
    ""{nameof(DeviceInfo.OperationSystemInfo)}"",
    ""{nameof(DeviceInfo.AppVersion)}"",
    ""{nameof(DeviceInfo.LastUpdate)}""
FROM
    {_dbName}
ORDER BY
    ""{nameof(DeviceInfo.LastUpdate)}"" DESC
LIMIT
    @{nameof(parameters.Limit)}
OFFSET
    @{nameof(parameters.Offset)};";

        var command = new CommandDefinition(commandText: query, parameters: parameters, cancellationToken: cancellationToken);

        var result = await session.QueryAsync<DeviceInfo>(command);
        return result.AsList();
    }

    public async Task<DeviceInfo?> GetDevice(IPgSession session, Guid id, CancellationToken cancellationToken)
    {
        var parameters = new
        {
            Id = id,
        };

        const string query = $@"
SELECT
    ""{nameof(DeviceInfo.Id)}"",
    ""{nameof(DeviceInfo.UserName)}"",
    ""{nameof(DeviceInfo.OperationSystemType)}"",
    ""{nameof(DeviceInfo.OperationSystemInfo)}"",
    ""{nameof(DeviceInfo.AppVersion)}"",
    ""{nameof(DeviceInfo.LastUpdate)}""
FROM
    {_dbName}
WHERE
    ""{nameof(DeviceInfo.Id)}"" = @{nameof(parameters.Id)};";

        var command = new CommandDefinition(commandText: query, parameters: parameters, cancellationToken: cancellationToken);

        var devices = await session.QueryAsync<DeviceInfo>(command);
        var result = devices.FirstOrDefault();
        return result;
    }
}
