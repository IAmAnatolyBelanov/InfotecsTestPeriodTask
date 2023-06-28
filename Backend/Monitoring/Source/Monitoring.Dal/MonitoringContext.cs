using Infotecs.Monitoring.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Monitoring.Dal;
public class MonitoringContext : DbContext, IMonitoringContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("AppDb");
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<DeviceInfo> Devices { get; set; }
    public DbSet<LoginInfo> Logins { get; set; }
}
