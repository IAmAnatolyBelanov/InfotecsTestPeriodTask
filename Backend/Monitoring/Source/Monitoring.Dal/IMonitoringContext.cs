using Infotecs.Monitoring.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Monitoring.Dal;
public interface IMonitoringContext : IDisposable, IAsyncDisposable
{
    DbSet<DeviceInfo> Devices { get; }
    DbSet<LoginInfo> Logins { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
