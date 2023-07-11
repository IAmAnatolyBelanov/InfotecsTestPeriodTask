using Monitoring.Dal;
using Monitoring.Dal.Models;
using Monitoring.Shared.DateTimeProviders;
using Monitoring.Shared.Paginations;

namespace Monitoring.Domain.DeviceBizRules;

/// <inheritdoc cref="IDeviceService"/>
public class DeviceService : IDeviceService
{
    private readonly ISessionFactory _sessionFactory;
    private readonly IPgDeviceRepository _deviceRepository;

    /// <summary>
    /// Конструктор класса <see cref="DeviceService"/>.
    /// </summary>
    /// <param name="sessionFactory"><see cref="ISessionFactory"/>.</param>
    /// <param name="deviceRepository"><see cref="IPgDeviceRepository"/></param>
    public DeviceService(ISessionFactory sessionFactory, IPgDeviceRepository deviceRepository)
    {
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
