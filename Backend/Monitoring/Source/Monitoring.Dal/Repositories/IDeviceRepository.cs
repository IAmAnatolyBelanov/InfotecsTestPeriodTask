using Infotecs.Monitoring.Dal.Models;
using Infotecs.Monitoring.Shared.Paginations;
using Monitoring.Dal.Sessions;

namespace Monitoring.Dal.Repositories;
public interface IDeviceRepository
{
    Task<DeviceInfo?> GetDevice(IPgSession session, Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<DeviceInfo>> GetDevices(IPgSession session, Pagination pagination, CancellationToken cancellationToken);

    Task MergeDevice(IPgSession session, DeviceInfo device, CancellationToken cancellationToken);
}
