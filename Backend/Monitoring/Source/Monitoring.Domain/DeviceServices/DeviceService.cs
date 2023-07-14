using Monitoring.Dal.Models;
using Monitoring.Dal.Repositories;
using Monitoring.Dal.Sessions;
using Monitoring.Shared.Paginations;

namespace Monitoring.Domain.DeviceServices;

/// <inheritdoc cref="IDeviceService"/>
public class DeviceService : IDeviceService
{
    private readonly ISessionFactory _sessionFactory;
    private readonly IDeviceRepository _deviceRepository;

    /// <summary>
    /// Конструктор класса <see cref="DeviceService"/>.
    /// </summary>
    /// <param name="sessionFactory"><see cref="ISessionFactory"/>.</param>
    /// <param name="deviceRepository"><see cref="IDeviceRepository"/></param>
    public DeviceService(ISessionFactory sessionFactory, IDeviceRepository deviceRepository)
    {
        _sessionFactory = sessionFactory;
        _deviceRepository = deviceRepository;
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
    public async Task AddOrUpdateDevice(DeviceInfo device, CancellationToken cancellationToken)
    {
        await using(var session = _sessionFactory.CreateSession(beginTransaction: true))
        {
            await _deviceRepository.InsertOrUpdateDevice(session, device, cancellationToken);
            await session.CommitAsync(cancellationToken);
        }
    }
}
