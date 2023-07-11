using Dapper;
using Infotecs.Monitoring.Dal.Models;
using Infotecs.Monitoring.Shared.Paginations;
using Infotecs.Monitoring.Dal.Sessions;

namespace Infotecs.Monitoring.Dal.Repositories;

/// <inheritdoc cref="IPgDeviceRepository"/>
public class PgDeviceRepository : IPgDeviceRepository
{
    private const string DbName = "public.devices";


    /// <inheritdoc/>
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

        const string Query = $@"
MERGE INTO {DbName} AS TARGET
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

        await session.ExecuteAsync(Query, parameters, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<DeviceInfo>> GetDevices(IPgSession session, Pagination pagination, CancellationToken cancellationToken)
    {
        var parameters = new
        {
            Limit = pagination.PageSize,
            Offset = pagination.PageIndex * pagination.PageSize,
        };

        const string Query = $@"
SELECT
    ""{nameof(DeviceInfo.Id)}"",
    ""{nameof(DeviceInfo.UserName)}"",
    ""{nameof(DeviceInfo.OperationSystemType)}"",
    ""{nameof(DeviceInfo.OperationSystemInfo)}"",
    ""{nameof(DeviceInfo.AppVersion)}"",
    ""{nameof(DeviceInfo.LastUpdate)}""
FROM
    {DbName}
ORDER BY
    ""{nameof(DeviceInfo.LastUpdate)}"" DESC
LIMIT
    @{nameof(parameters.Limit)}
OFFSET
    @{nameof(parameters.Offset)};";

        var result = await session.QueryAsync<DeviceInfo>(Query, parameters, cancellationToken);
        return result.AsList();
    }

    /// <inheritdoc/>
    public async Task<DeviceInfo?> GetDevice(IPgSession session, Guid id, CancellationToken cancellationToken)
    {
        var parameters = new
        {
            Id = id,
        };

        const string Query = $@"
SELECT
    ""{nameof(DeviceInfo.Id)}"",
    ""{nameof(DeviceInfo.UserName)}"",
    ""{nameof(DeviceInfo.OperationSystemType)}"",
    ""{nameof(DeviceInfo.OperationSystemInfo)}"",
    ""{nameof(DeviceInfo.AppVersion)}"",
    ""{nameof(DeviceInfo.LastUpdate)}""
FROM
    {DbName}
WHERE
    ""{nameof(DeviceInfo.Id)}"" = @{nameof(parameters.Id)};";

        var devices = await session.QueryAsync<DeviceInfo>(Query, parameters, cancellationToken);

        var result = devices.FirstOrDefault();
        return result;
    }
}
