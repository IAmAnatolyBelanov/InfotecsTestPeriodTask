using Monitoring.Dal.Models;
using Monitoring.Dal.Repositories;
using Monitoring.Dal.Sessions;
using Monitoring.Shared.GuidProviders;

namespace Monitoring.Domain.DeviceEventServices;

/// <inheritdoc cref="IDeviceEventService"/>.
public class DeviceEventService : IDeviceEventService
{
    private readonly ISessionFactory _sessionFactory;
    private readonly IEventRepository _eventRepository;
    private readonly IGuidProvider _guidProvider;

    /// <summary>
    /// Конструктор класса <see cref="DeviceEventService"/>.
    /// </summary>
    /// <param name="sessionFactory"><see cref="ISessionFactory"/>.</param>
    /// <param name="eventRepository"><see cref="IEventRepository"/>.</param>
    /// <param name="guidProvider"><see cref="IGuidProvider"/>.</param>
    public DeviceEventService(ISessionFactory sessionFactory, IEventRepository eventRepository, IGuidProvider guidProvider)
    {
        _sessionFactory = sessionFactory;
        _eventRepository = eventRepository;
        _guidProvider = guidProvider;
    }

    /// <inheritdoc/>
    public async Task AddEvents(IReadOnlyCollection<DeviceEvent> deviceEvents, CancellationToken cancellationToken)
    {
        foreach (var deviceEvent in deviceEvents)
        {
            deviceEvent.Id = _guidProvider.NewGuid();
        }

        await using (var session = _sessionFactory.CreateSession(beginTransaction: true))
        {
            await _eventRepository.AddEvents(session, deviceEvents, cancellationToken);
            await session.CommitAsync(cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<DeviceEvent>> GetEventsByDevice(Guid deviceId, CancellationToken cancellationToken)
    {
        await using (var session = _sessionFactory.CreateSession())
        {
            var result = await _eventRepository.GetEventsByDevice(session, deviceId, cancellationToken);
            return result;
        }
    }
}
