using Infotecs.Monitoring.Dal;
using Infotecs.Monitoring.Shared.Paginations;

namespace Infotecs.Monitoring.Bll.DeviceBizRules;
public interface IDeviceBizRule
{
    ValueTask<IReadOnlyList<DeviceInfo>> GetAll(Pagination pagination, CancellationToken cancellationToken);
    ValueTask<DeviceStatistics> GetFullStatistics(Guid deviceId, CancellationToken cancellationToken);
    ValueTask<DeviceStatistics> GetStatistics(Guid deviceId, DateTimeOffset dateFrom, DateTimeOffset dateTo, CancellationToken cancellationToken);
    ValueTask<Guid> RegisterDevice(DeviceInfo device, CancellationToken cancellationToken);
}
