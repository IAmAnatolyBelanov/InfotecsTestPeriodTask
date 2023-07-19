using Monitoring.Dal.Models;
using Monitoring.Dal.Repositories;
using Monitoring.Dal.Sessions;
using Monitoring.Domain.DeviceEventServices;
using Monitoring.Shared.Extensions;
using Monitoring.Shared.Paginations;

namespace Monitoring.Domain.DeviceServices;

/// <inheritdoc cref="IDeviceService"/>
public class DeviceService : IDeviceService
{
    private readonly ISessionFactory _sessionFactory;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDeviceEventService _deviceEventService;

    /// <summary>
    /// Конструктор класса <see cref="DeviceService"/>.
    /// </summary>
    /// <param name="sessionFactory"><see cref="ISessionFactory"/>.</param>
    /// <param name="deviceRepository"><see cref="IDeviceRepository"/>.</param>
    /// <param name="deviceEventService"><see cref="IDeviceEventService"/>.</param>
    public DeviceService(ISessionFactory sessionFactory, IDeviceRepository deviceRepository, IDeviceEventService deviceEventService)
    {
        _sessionFactory = sessionFactory;
        _deviceRepository = deviceRepository;
        _deviceEventService = deviceEventService;
    }

    /// <inheritdoc/>
    public async Task<DeviceInfo?> Get(Guid id, CancellationToken cancellationToken)
    {
        await using (var session = _sessionFactory.CreateSession())
        {
            var result = await _deviceRepository.GetDevice(session, id, cancellationToken);
            return result;
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<DeviceInfo>> GetAll(Pagination pagination, CancellationToken cancellationToken)
    {
        await using (var session = _sessionFactory.CreateSession())
        {
            var result = await _deviceRepository.GetDevices(session, pagination, cancellationToken);
            return result;
        }
    }

    /// <inheritdoc/>
    public async Task<DeviceStatistics> GetStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        await using (var session = _sessionFactory.CreateSession())
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
    public async Task AddOrUpdateDevice(DeviceInfo device, IReadOnlyList<DeviceEvent> events, CancellationToken cancellationToken)
    {
        await using (var session = _sessionFactory.CreateSession(beginTransaction: true))
        {
            await _deviceRepository.InsertOrUpdateDevice(session, device, cancellationToken);

            if (events.HasSome())
            {
                await _deviceEventService.AddEvents(session, events, cancellationToken);
            }

            await session.CommitAsync(cancellationToken);
        }
    }
}
