namespace Infotecs.Monitoring.Dal.Models;

public class LoginInfo
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public DeviceInfo? Device { get; set; }
    public string UserName { get; set; } = default!;
    public string ApplicationVersion { get; set; } = default!;
    public DateTimeOffset DateTime { get; set; }
}
