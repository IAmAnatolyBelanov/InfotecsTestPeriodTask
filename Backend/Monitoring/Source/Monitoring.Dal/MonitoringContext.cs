using Microsoft.EntityFrameworkCore;

namespace Infotecs.Monitoring.Dal;
public class MonitoringContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("AppDb");
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<DeviceInfo> Devices { get; set; }
    public DbSet<LoginInfo> Logins { get; set; }
}

public class LoginInfo
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public DeviceInfo? Device { get; set; }
    public string UserName { get; set; } = default!;
    public string ApplicationVersion { get; set; } = default!;
    public DateTimeOffset DateTime { get; set; }
}

public class DeviceInfo
{
    public Guid Id { get; set; }
    public OperationSystemType OperationSystemType { get; set; }
    public string OperationSystemInfo { get; set; } = default!;
    public DateTimeOffset RegistrationDate { get; set; }
}

public enum OperationSystemType
{
    None,
    Windows,
    Linux,
    MacOs,
    Android,
    Ios,
}
