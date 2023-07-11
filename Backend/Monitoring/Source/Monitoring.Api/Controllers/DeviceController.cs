using Infotecs.Monitoring.Api.Infrastructure;
using Infotecs.Monitoring.Contracts.DeviceInfo;
using Infotecs.Monitoring.Domain.DeviceBizRules;
using Infotecs.Monitoring.Domain.Mappers;
using Infotecs.Monitoring.Shared.Paginations;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.Monitoring.Api.Controllers;

/// <summary>
/// Контроллер для работы с девайсами.
/// </summary>
[ApiController]
[Route("Devices")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly ILogger<DeviceController> _logger;
    private readonly IDeviceInfoMapper _deviceInfoMapper;

    /// <summary>
    /// Конструктор класса <see cref="DeviceController"/>.
    /// </summary>
    /// <param name="deviceService">Бизнес-логика работы с девайсами.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="deviceInfoMapper">Маппер для DeviceInfo.</param>
    public DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger, IDeviceInfoMapper deviceInfoMapper)
    {
        _deviceService = deviceService;
        _logger = logger;
        _deviceInfoMapper = deviceInfoMapper;
    }

    /// <summary>
    /// Регистрирует девайс в системе.
    /// </summary>
    /// <param name="device">Девайс, что нужно зарегистрировать в системе.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Пустой ответ в случае успешной регистрации, или информацию об ошибке в случае её возникновения.</returns>
    [HttpPost("RegisterDevice")]
    public async Task<BaseResponse<object>> RegisterDevice(DeviceInfoDto device, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start to register device {device.Id}.");
        var internalDevice = _deviceInfoMapper.MapFromDto(device);

        await _deviceService.AddOrUpdateDevice(internalDevice, cancellationToken);

        return BaseResponseExtensions.EmptySuccess();
    }

    /// <summary>
    /// Возвращает информацию о девайсах.
    /// </summary>
    /// <param name="pagination">Пагинация.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Массив данных о девайсах или информацию об ошибке в случае её возникновения.</returns>
    [HttpGet]
    public async Task<BaseResponse<IReadOnlyList<DeviceInfoDto>>> GetAll([FromQuery]Pagination pagination, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start to get info about all devices.");
        var result = await _deviceService.GetAll(pagination, cancellationToken);

        IReadOnlyList<DeviceInfoDto> resultDto = result.Select(_deviceInfoMapper.MapToDto).ToArray();

        return resultDto.ToResponse();
    }

    /// <summary>
    /// Возвращает статистику по запрошенному девайсу.
    /// </summary>
    /// <param name="deviceId">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Статистика по запрошенному девайсу или информация об ошибке в случае её возникновения.</returns>
    [HttpPost("GetStatistics")]
    public async Task<BaseResponse<DeviceStatistics>> GetStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start to get statistics about device {deviceId}.");
        var result = await _deviceService.GetStatistics(deviceId, cancellationToken);
        return result.ToResponse();
    }
}
