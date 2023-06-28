namespace Infotecs.Monitoring.Dal.Models;

public class DeviceInfo
{
    public Guid Id { get; set; }
    public OperationSystemType OperationSystemType { get; set; }
    public string OperationSystemInfo { get; set; } = default!;
    public DateTimeOffset RegistrationDate { get; set; }
}
