using Microsoft.AspNetCore.Mvc;
using Monitoring.Api.Infrastructure;
using Monitoring.Contracts.DeviceEvents;
using Monitoring.Domain.DeviceEventServices;
using Monitoring.Domain.Mappers;

namespace Monitoring.Api.Controllers;

/// <summary>
/// Контроллер для работы с событиями.
/// </summary>
[ApiController]
[Route("events")]
public class EventController
{
    private readonly IDeviceEventService _deviceEventService;
    private readonly IDeviceEventLightMapper _deviceEventLightMapper;
    private readonly IDeviceEventMapper _deviceEventMapper;

    /// <summary>
    /// Конструктор класса <see cref="EventController"/>.
    /// </summary>
    /// <param name="deviceEventService"><see cref="IDeviceEventService"/>.</param>
    /// <param name="deviceEventLightMapper"><see cref="IDeviceEventLightMapper"/>.</param>
    /// <param name="deviceEventMapper"><see cref="IDeviceEventMapper"/>.</param>
    public EventController(
        IDeviceEventService deviceEventService,
        IDeviceEventLightMapper deviceEventLightMapper,
        IDeviceEventMapper deviceEventMapper)
    {
        _deviceEventService = deviceEventService;
        _deviceEventLightMapper = deviceEventLightMapper;
        _deviceEventMapper = deviceEventMapper;
    }

    /// <summary>
    /// Добавляет событие в БД.
    /// </summary>
    /// <param name="deviceEvent">Событие.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Id добавленного события.</returns>
    [HttpPut]
    public async Task<BaseResponse<Guid>> AddEvent(DeviceEventDto deviceEvent, CancellationToken cancellationToken)
    {
        var @event = _deviceEventMapper.MapFromDto(deviceEvent);
        var result = await _deviceEventService.AddEvent(@event, cancellationToken);
        return result.ToResponse();
    }

    /// <summary>
    /// Возвращает список событий указанного девайса.
    /// </summary>
    /// <param name="deviceId">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Список событий указанного девайса.</returns>
    /// <remarks>Если девайса не существует, список будет пустым.</remarks>
    [HttpGet("by-device-id")]
    public async Task<BaseResponse<EventCollection>> GetEventsByDevice(Guid deviceId, CancellationToken cancellationToken)
    {
        var events = await _deviceEventService.GetEventsByDevice(deviceId, cancellationToken);
        var result = new EventCollection
        {
            DeviceId = deviceId,
            Events = events.Select(_deviceEventLightMapper.MapToDto).ToArray(),
        };
        return result.ToResponse();
    }
}
