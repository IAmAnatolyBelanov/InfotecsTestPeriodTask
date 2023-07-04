using Infotecs.Monitoring.Api.Infrastructure;
using Infotecs.Monitoring.Dal.Models;
using Infotecs.Monitoring.Domain.DeviceBizRules;
using Infotecs.Monitoring.Shared.Paginations;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.Monitoring.Api.Controllers;

/// <summary>
/// Контроллер для работы с девайсами.
/// </summary>
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly ILogger<DeviceController> _logger;

    /// <summary>
    /// Конструктор класса <see cref="DeviceController"/>.
    /// </summary>
    /// <param name="deviceService">Бизнес-логика работы с девайсами.</param>
    /// <param name="logger">Логгер.</param>
    public DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger)
    {
        _deviceService = deviceService;
        _logger = logger;
    }

    /// <summary>
    /// Регистрирует девайс в системе.
    /// </summary>
    /// <param name="device">Девайс, что нужно зарегистрировать в системе.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Пустой ответ в случае успешной регистрации, или информацию об ошибке в случае её возникновения.</returns>
    [HttpPost("Device/RegisterDevice")]
    public async Task<BaseResponse<object>> RegisterDevice(DeviceInfo device, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start to register device {device.Id}.");
        await _deviceService.AddOrUpdateDevice(device, cancellationToken);
        return BaseResponseExtensions.EmptySuccess();
    }

    /// <summary>
    /// Возвращает информацию о девайсах.
    /// </summary>
    /// <param name="pagination">Пагинация.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Массив данных о девайсах или информацию об ошибке в случае её возникновения.</returns>
    [HttpGet("Device")]
    public async Task<BaseResponse<IReadOnlyList<DeviceInfo>>> GetAll([FromQuery]Pagination pagination, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start to get info about all devices.");
        var result = await _deviceService.GetAll(pagination, cancellationToken);
        return result.ToResponse();
    }

    /// <summary>
    /// Возвращает статистику по запрошенному девайсу.
    /// </summary>
    /// <param name="deviceId">Id девайса.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Статистика по запрошенному девайсу или информация об ошибке в случае её возникновения.</returns>
    [HttpPost("Device/GetStatistics")]
    public async Task<BaseResponse<DeviceStatistics>> GetStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Start to get statistics about device {deviceId}.");
        var result = await _deviceService.GetStatistics(deviceId, cancellationToken);
        return result.ToResponse();
    }
}
