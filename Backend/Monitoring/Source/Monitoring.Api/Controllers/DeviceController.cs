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

    /// <summary>
    /// Конструктор класса <see cref="DeviceController"/>.
    /// </summary>
    /// <param name="deviceService">Бизнес-логика работы с девайсами.</param>
    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    /// <summary>
    /// Регистрирует девайс в системе.
    /// </summary>
    /// <param name="device">Девайс, что нужно зарегистрировать в системе.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Пустой ответ в случае успешной регистрации, или информацию об ошибке в случае её возникновения.</returns>
    [HttpPost("Device/RegisterDevice")]
    public async ValueTask<BaseResponse<object>> RegisterDevice(DeviceInfo device, CancellationToken cancellationToken)
    {
        await _deviceService.AddOrUpdateDevice(device, cancellationToken);
        return BaseResponseExtensions.EmptySuccess();
    }

    /// <summary>
    /// Возвращает информацию о девайсах.
    /// </summary>
    /// <param name="pagination">Пагинация.</param>
    /// <param name="cancellationToken">Токен для отмены запроса.</param>
    /// <returns>Массив данных о девайсах или информацию об ошибке в случае её возникновения.</returns>
    [HttpPost("Device/GetAll")]
    public async ValueTask<BaseResponse<IReadOnlyList<DeviceInfo>>> GetAll(Pagination pagination, CancellationToken cancellationToken)
    {
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
    public async ValueTask<BaseResponse<DeviceStatistics>> GetStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        var result = await _deviceService.GetStatistics(deviceId, cancellationToken);
        return result.ToResponse();
    }
}
