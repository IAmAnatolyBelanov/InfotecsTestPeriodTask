using Microsoft.AspNetCore.Mvc;
using Monitoring.Api.Infrastructure;
using Monitoring.Contracts.Dtos.DeviceInfo;
using Monitoring.Contracts.Queries.Device;
using Monitoring.Dal.Models;
using Monitoring.Domain.DeviceServices;
using Monitoring.Domain.Mappers;
using Monitoring.Shared.Extensions;
using Monitoring.Shared.GuidProviders;
using Monitoring.Shared.Paginations;

namespace Monitoring.Api.Controllers;

/// <summary>
/// Контроллер для работы с девайсами.
/// </summary>
[ApiController]
[Route("api/devices")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly ILogger<DeviceController> _logger;
    private readonly IDeviceInfoMapper _deviceInfoMapper;
    private readonly IDeviceEventLightMapper _deviceEventMapper;
    private readonly IGuidProvider _guidProvider;

    /// <summary>
    /// Конструктор класса <see cref="DeviceController"/>.
    /// </summary>
    /// <param name="deviceService"><see cref="IDeviceService"/>.</param>
    /// <param name="logger"><see cref="ILogger{TCategoryName}"/>.</param>
    /// <param name="deviceInfoMapper"><see cref="IDeviceInfoMapper"/>.</param>
    /// <param name="deviceEventMapper"><see cref="IDeviceEventLightMapper"/>.</param>
    /// <param name="guidProvider"><see cref="IGuidProvider"/>.</param>
    public DeviceController(
        IDeviceService deviceService,
        ILogger<DeviceController> logger,
        IDeviceInfoMapper deviceInfoMapper,
        IDeviceEventLightMapper deviceEventMapper,
        IGuidProvider guidProvider)
    {
        _deviceService = deviceService;
        _logger = logger;
        _deviceInfoMapper = deviceInfoMapper;
        _deviceEventMapper = deviceEventMapper;
        _guidProvider = guidProvider;
    }

    /// <summary>
    /// Регистрирует девайс в системе.
    /// </summary>
    /// <param name="device">Девайс, что нужно зарегистрировать в системе.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Пустой ответ в случае успешной регистрации, или информацию об ошибке в случае её возникновения.</returns>
    [HttpPost("register-device")]
    [Obsolete($"Use {nameof(RegisterDeviceWithEvents)} instead.")]
    public async Task<BaseResponse<object>> RegisterDevice(DeviceInfoDto device, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start to register device {device.Id}.");
        var query = new RegisterDeviceQuery { Device = device };

        await RegisterDeviceWithEvents(query, cancellationToken);

        return BaseResponseExtensions.EmptySuccess();
    }

    /// <summary>
    /// Возвращает информацию о девайсах.
    /// </summary>
    /// <param name="pagination">Пагинация.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Массив данных о девайсах или информацию об ошибке в случае её возникновения.</returns>
    [HttpGet]
    public async Task<BaseResponse<IReadOnlyList<DeviceInfoDto>>> GetAll([FromQuery] Pagination pagination, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start to get info about all devices.");
        var result = await _deviceService.GetAll(pagination, cancellationToken);

        IReadOnlyList<DeviceInfoDto> resultDto = result.Select(_deviceInfoMapper.MapToDto).ToArray();

        return resultDto.ToResponse();
    }

    /// <summary>
    /// Возвращает информацию о девайсе.
    /// </summary>
    /// <param name="id">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Информацию о девайсе.</returns>
    /// <remarks>Если девайса не существует, вернёт null.</remarks>
    [HttpGet("{id}")]
    public async Task<BaseResponse<DeviceInfoDto>> GetDevice(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start to get info about device {id}.");
        var device = await _deviceService.Get(id, cancellationToken);

        var result = _deviceInfoMapper.MapToDto(device);

        return result.ToResponse();
    }

    /// <summary>
    /// Возвращает статистику по запрошенному девайсу.
    /// </summary>
    /// <param name="deviceId">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Статистика по запрошенному девайсу или информация об ошибке в случае её возникновения.</returns>
    [HttpPost("get-statistics")]
    public async Task<BaseResponse<DeviceStatistics>> GetStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start to get statistics about device {deviceId}.");
        var result = await _deviceService.GetStatistics(deviceId, cancellationToken);
        return result.ToResponse();
    }

    /// <summary>
    /// Регистрирует девайс в системе и сохраняет события.
    /// </summary>
    /// <param name="query">Девайс и его события, что нужно зарегистрировать в системе.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>'Ok' в случае успеха, или информацию об ошибке в случае её возникновения.</returns>
    [HttpPut]
    public async Task<BaseResponse<string>> RegisterDeviceWithEvents(RegisterDeviceQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start to register device {query.Device.Id} and {query.Events?.Count ?? 0} events.");

        var internalDevice = _deviceInfoMapper.MapFromDto(query.Device);
        var internalEvents = query.Events.HasSome()
            ? query.Events!.Select(_deviceEventMapper.MapFromDto).ToArray()
            : Array.Empty<DeviceEvent>();

        for (var i = 0; i < internalEvents.Length; i++)
        {
            var @event = internalEvents[i];
            @event.DeviceId = internalDevice.Id;
            @event.Id = _guidProvider.NewGuid();
        }

        await _deviceService.AddOrUpdateDevice(internalDevice, internalEvents, cancellationToken);

        return "Ok".ToResponse();
    }
}
