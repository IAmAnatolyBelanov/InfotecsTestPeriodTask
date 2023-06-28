using Monitoring.Dal;

namespace Infotecs.Monitoring.Bll.DeviceBizRules;
public interface IDeviceBizRule
{
    ValueTask<IReadOnlyList<DeviceInfo>> GetAll(CancellationToken cancellationToken);
    ValueTask<Statistics> GetFullStatistics(Guid deviceId, CancellationToken cancellationToken);
    ValueTask<Statistics> GetStatistics(Guid deviceId, DateTimeOffset dateFrom, DateTimeOffset dateTo, CancellationToken cancellationToken);
    ValueTask<Guid> RegisterDevice(DeviceInfo device, CancellationToken cancellationToken);
}