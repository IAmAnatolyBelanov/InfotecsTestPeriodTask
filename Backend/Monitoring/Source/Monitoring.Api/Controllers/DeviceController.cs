using System.Data.SqlTypes;
using Infotecs.Monitoring.Api.Controllers;
using Infotecs.Monitoring.Bll.DeviceBizRules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitoring.Dal;

namespace Monitoring.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> _logger;
    private readonly IDeviceBizRule _deviceBizRule;

    public DeviceController(ILogger<DeviceController> logger, IDeviceBizRule deviceBizRule)
    {
        _logger = logger;
        _deviceBizRule = deviceBizRule;
    }

    [HttpPost]
    public async ValueTask<Guid> RegisterDevice(DeviceInfo device, CancellationToken cancellationToken)
    {
        var result = await _deviceBizRule.RegisterDevice(device, cancellationToken);
        return result;
    }

    [HttpGet]
    public async ValueTask<IReadOnlyList<DeviceInfo>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _deviceBizRule.GetAll(cancellationToken);
        return result;
    }

    [HttpGet]
    public async ValueTask<Statistics> GetFullStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        var result = await _deviceBizRule.GetFullStatistics(deviceId, cancellationToken);
        return result;
    }

    [HttpGet]
    public async ValueTask<Statistics> GetStatistics(Guid deviceId, DateTimeOffset dateFrom, DateTimeOffset dateTo, CancellationToken cancellationToken)
    {
        var result = await _deviceBizRule.GetStatistics(deviceId, dateFrom, dateTo, cancellationToken);
        return result;
    }
}
