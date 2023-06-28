using System.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Monitoring.Dal;
using Monitoring.Shared.Exceptions;

namespace Infotecs.Monitoring.Bll.DeviceBizRules;
public class DeviceBizRule : IDeviceBizRule, IDisposable, IAsyncDisposable
{
    private readonly MonitoringContext _context;
    private readonly ILogger<DeviceBizRule> _logger;

    public DeviceBizRule(MonitoringContext monitoringContext, ILogger<DeviceBizRule> logger)
    {
        _context = monitoringContext;
        _logger = logger;
    }

    public async ValueTask<IReadOnlyList<DeviceInfo>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _context.Devices.ToListAsync(cancellationToken);
        return result;
    }

    public async ValueTask<Statistics> GetFullStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        var logins = await GetStatistics(deviceId, SqlDateTime.MinValue.Value, DateTimeOffset.UtcNow, cancellationToken);

        return logins;
    }

    public async ValueTask<Statistics> GetStatistics(Guid deviceId, DateTimeOffset dateFrom, DateTimeOffset dateTo, CancellationToken cancellationToken)
    {
        var logins = await _context.Logins
            .Where(x => x.DeviceId == deviceId && x.DateTime >= dateFrom && x.DateTime < dateTo)
            .OrderByDescending(x => x.DateTime)
            .ToListAsync(cancellationToken);

        var statistics = new Statistics
        {
            DeviceId = deviceId,
            LastLogin = logins.First().DateTime,
            LoginCount = logins.Count,
            UniqueUserCount = logins.DistinctBy(l => l.UserName).Count(),
        };

        return statistics;
    }

    public async ValueTask<Guid> RegisterDevice(DeviceInfo device, CancellationToken cancellationToken)
    {
        if (device.Id != default)
            throw new ClientException("Не клиент выдаёт айдишник!");

        await _context.Devices.AddAsync(device, cancellationToken);
        await _context.SaveChangesAsync();

        return device.Id;
    }

    public void Dispose() => _context.Dispose();
    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
