using Infotecs.Monitoring.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Monitoring.Dal;

/// <inheritdoc cref="IMonitoringContext"/>
public class MonitoringContext : DbContext, IMonitoringContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("AppDb");
        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc/>
    public DbSet<DeviceInfo> Devices { get; set; }
}
