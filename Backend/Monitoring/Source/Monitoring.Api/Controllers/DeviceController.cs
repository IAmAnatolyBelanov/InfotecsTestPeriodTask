using System.Data.SqlTypes;
using Infotecs.Monitoring.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitoring.Dal;

namespace Monitoring.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> _logger;
    private readonly MonitoringContext _context;

    public DeviceController(ILogger<DeviceController> logger, MonitoringContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public async ValueTask<Guid> RegisterDevice(DeviceInfo device, CancellationToken cancellationToken)
    {
        if (device.Id != default)
            throw new Exception("Не клиент выдаёт айдишник!");

        await _context.Devices.AddAsync(device, cancellationToken);
        await _context.SaveChangesAsync();

        return device.Id;
    }

    [HttpGet]
    public async ValueTask<IReadOnlyList<DeviceInfo>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _context.Devices.ToListAsync(cancellationToken);
        return result;
    }

    [HttpGet]
    public async ValueTask<Statistics> GetFullStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        var logins = await GetStatistics(deviceId, SqlDateTime.MinValue.Value, DateTimeOffset.UtcNow, cancellationToken);

        return logins;
    }

    [HttpGet]
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

    public class Statistics
    {
        public Guid DeviceId { get; set; }
        public int LoginCount { get; set; }
        public int UniqueUserCount { get; set; }
        public DateTimeOffset LastLogin { get; set; }
    }
}
