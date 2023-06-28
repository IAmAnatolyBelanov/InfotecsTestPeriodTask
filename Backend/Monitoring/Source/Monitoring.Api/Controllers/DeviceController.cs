using System.Data.SqlTypes;
using Infotecs.Monitoring.Api.Controllers;
using Infotecs.Monitoring.Api.Infrastructure;
using Infotecs.Monitoring.Bll.DeviceBizRules;
using Infotecs.Monitoring.Dal;
using Infotecs.Monitoring.Dal.Models;
using Infotecs.Monitoring.Shared.Exceptions;
using Infotecs.Monitoring.Shared.Paginations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Monitoring.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceBizRule _deviceBizRule;

    public DeviceController(IDeviceBizRule deviceBizRule)
    {
        _deviceBizRule = deviceBizRule;
    }

    [HttpPost]
    public async ValueTask<BaseResponse<Guid>> RegisterDevice(DeviceInfo device, CancellationToken cancellationToken)
    {
        var result = await _deviceBizRule.RegisterDevice(device, cancellationToken);
        return result.ToResponse();
    }

    [HttpPost]
    public async ValueTask<BaseResponse<IReadOnlyList<DeviceInfo>>> GetAll(Pagination pagination, CancellationToken cancellationToken)
    {
        if (pagination.PageIndex < 0)
            throw new ClientException($"{nameof(pagination)}.{nameof(pagination.PageIndex)} must be >= 0.");
        if (pagination.PageSize <= 0)
            throw new ClientException($"{nameof(pagination)}.{nameof(pagination.PageSize)} must be > 0.");
        if (pagination.PageSize > 100)
            throw new ClientException($"{nameof(pagination)}.{nameof(pagination.PageSize)} must be <= 100.");

        var result = await _deviceBizRule.GetAll(pagination, cancellationToken);
        return result.ToResponse();
    }

    [HttpPost]
    public async ValueTask<BaseResponse<DeviceStatistics>> GetFullStatistics(Guid deviceId, CancellationToken cancellationToken)
    {
        var result = await _deviceBizRule.GetFullStatistics(deviceId, cancellationToken);
        return result.ToResponse();
    }

    [HttpPost]
    public async ValueTask<BaseResponse<DeviceStatistics>> GetStatistics(Guid deviceId, DateTimeOffset dateFrom, DateTimeOffset dateTo, CancellationToken cancellationToken)
    {
        var result = await _deviceBizRule.GetStatistics(deviceId, dateFrom, dateTo, cancellationToken);
        return result.ToResponse();
    }
}
