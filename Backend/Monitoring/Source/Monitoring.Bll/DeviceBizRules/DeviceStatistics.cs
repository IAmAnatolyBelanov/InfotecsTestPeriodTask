namespace Infotecs.Monitoring.Bll.DeviceBizRules;
public class DeviceStatistics
{
    public Guid DeviceId { get; set; }
    public int LoginCount { get; set; }
    public int UniqueUserCount { get; set; }
    public DateTimeOffset? LastLogin { get; set; }
}
