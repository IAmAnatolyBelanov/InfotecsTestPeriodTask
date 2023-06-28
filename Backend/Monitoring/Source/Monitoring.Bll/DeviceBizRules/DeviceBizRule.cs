using System.Data.SqlTypes;
using Infotecs.Monitoring.Dal;
using Infotecs.Monitoring.Dal.Models;
using Infotecs.Monitoring.Shared.DateTimeProviders;
using Infotecs.Monitoring.Shared.Paginations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infotecs.Monitoring.Bll.DeviceBizRules;
public class DeviceBizRule : IDeviceBizRule, IDisposable, IAsyncDisposable
{
    private readonly IMonitoringContext _context;
    private readonly IClock _clock;

    public DeviceBizRule(IMonitoringContext monitoringContext, IClock clock)
    {
        _context = monitoringContext;
        _clock = clock;
    }

    public async ValueTask<IReadOnlyList<DeviceInfo>> GetAll(Pagination pagination, CancellationToken cancellationToken)
    {
        var result = await _context.Devices
            .OrderByDescending(x => x.RegistrationDate)
            .Skip(pagination.PageIndex * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return result;
    }

    public async ValueTask<DeviceStatistics> GetFullStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        var statistics = await GetStatistics(deviceId, SqlDateTime.MinValue.Value, _clock.UtcNow, cancellationToken);

        return statistics;
    }

    public async ValueTask<DeviceStatistics> GetStatistics(Guid deviceId, DateTimeOffset dateFrom, DateTimeOffset dateTo, CancellationToken cancellationToken)
    {
        var logins = await _context.Logins
            .Where(x => x.DeviceId == deviceId && x.DateTime >= dateFrom && x.DateTime < dateTo)
            .OrderByDescending(x => x.DateTime)
            .ToListAsync(cancellationToken);

        var statistics = new DeviceStatistics
        {
            DeviceId = deviceId,
            LastLogin = logins.FirstOrDefault()?.DateTime,
            LoginCount = logins.Count,
            UniqueUserCount = logins.DistinctBy(l => l.UserName).Count(),
        };

        return statistics;
    }

    public async ValueTask<Guid> RegisterDevice(DeviceInfo device, CancellationToken cancellationToken)
    {
        device.Id = Guid.NewGuid();
        device.RegistrationDate = _clock.UtcNow;

        await _context.Devices.AddAsync(device, cancellationToken);
        await _context.SaveChangesAsync();

        return device.Id;
    }

    public void Dispose() => _context.Dispose();
    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
