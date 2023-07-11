using Infotecs.Monitoring.Dal;
using Infotecs.Monitoring.Dal.Models;
using Infotecs.Monitoring.Shared.DateTimeProviders;
using Infotecs.Monitoring.Shared.Paginations;
using Monitoring.Dal.Repositories;
using Monitoring.Dal.Sessions;

namespace Infotecs.Monitoring.Domain.DeviceBizRules;

/// <inheritdoc cref="IDeviceService"/>
public class DeviceService : IDeviceService
{
    private readonly IMonitoringContext _context;
    private readonly IClock _clock;
    private readonly ISessionFactory _sessionFactory;
    private readonly IDeviceRepository _deviceRepository;

    /// <summary>
    /// Конструктор класса <see cref="DeviceService"/>.
    /// </summary>
    /// <param name="monitoringContext">Контекст работы с БД.</param>
    /// <param name="clock">Абстракция над <see cref="DateTimeOffset"/>.</param>
    public DeviceService(IMonitoringContext monitoringContext, IClock clock, ISessionFactory sessionFactory, IDeviceRepository deviceRepository)
    {
        _context = monitoringContext;
        _clock = clock;
        _sessionFactory = sessionFactory;
        _deviceRepository = deviceRepository;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<DeviceInfo>> GetAll(Pagination pagination, CancellationToken cancellationToken)
    {
        await using (var session = _sessionFactory.CreateForPostgres())
        {
            var result = await _deviceRepository.GetDevices(session, pagination, cancellationToken);
            return result;
        }
    }

    /// <inheritdoc/>
    public async Task<DeviceStatistics> GetStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        await using (var session = _sessionFactory.CreateForPostgres())
        {
            var device = await _deviceRepository.GetDevice(session, deviceId, cancellationToken);

            var statistics = new DeviceStatistics
            {
                DeviceId = deviceId,
                LastUpdate = device?.LastUpdate,
            };

            return statistics;
        }
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateDevice(DeviceInfo device, CancellationToken cancellationToken)
    {
        await using(var session = _sessionFactory.CreateForPostgres(beginTransaction: true))
        {
            await _deviceRepository.MergeDevice(session, device, cancellationToken);
            await session.CommitAsync(cancellationToken);
        }
    }
}
