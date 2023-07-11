using Monitoring.Dal;
using Monitoring.Dal.Models;
using Monitoring.Shared.DateTimeProviders;
using Monitoring.Shared.Paginations;
using Microsoft.EntityFrameworkCore;

namespace Monitoring.Domain.DeviceBizRules;

/// <inheritdoc cref="IDeviceService"/>
public class DeviceService : IDeviceService
{
    private readonly IMonitoringContext _context;
    private readonly IClock _clock;

    /// <summary>
    /// Конструктор класса <see cref="DeviceService"/>.
    /// </summary>
    /// <param name="monitoringContext">Контекст работы с БД.</param>
    /// <param name="clock">Абстракция над <see cref="DateTimeOffset"/>.</param>
    public DeviceService(IMonitoringContext monitoringContext, IClock clock)
    {
        _context = monitoringContext;
        _clock = clock;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<DeviceInfo>> GetAll(Pagination pagination, CancellationToken cancellationToken)
    {
        var result = await _context.Devices
            .OrderByDescending(x => x.LastUpdate)
            .Skip(pagination.PageIndex * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc/>
    public async Task<DeviceStatistics> GetStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await _context.Devices
            .FirstOrDefaultAsync(x => x.Id == deviceId);

        var statistics = new DeviceStatistics
        {
            DeviceId = deviceId,
            LastUpdate = device?.LastUpdate,
        };

        return statistics;
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateDevice(DeviceInfo device, CancellationToken cancellationToken)
    {
        device.LastUpdate = _clock.UtcNow;

        if (await _context.Devices.AnyAsync(x => x.Id == device.Id))
        {
            _context.Devices.Update(device);
        }
        else
        {
            await _context.Devices.AddAsync(device);
        }

        await _context.SaveChangesAsync();
    }
}
