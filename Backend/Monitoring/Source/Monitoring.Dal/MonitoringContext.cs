using Infotecs.Monitoring.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Monitoring.Dal;

/// <inheritdoc cref="IMonitoringContext"/>
public class MonitoringContext : DbContext, IMonitoringContext
{
    /// <inheritdoc/>
    public DbSet<DeviceInfo> Devices { get; set; }

    /// <summary>
    /// Конфигурирует контекст БД.
    /// </summary>
    /// <param name="optionsBuilder">Конфигурация контекста БД.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("AppDb");
        base.OnConfiguring(optionsBuilder);
    }
}
