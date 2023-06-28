namespace Infotecs.Monitoring.Shared.DateTimeProviders;
public class Clock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    public DateTimeOffset Now => DateTimeOffset.Now;
}
