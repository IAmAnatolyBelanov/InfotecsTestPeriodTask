namespace Infotecs.Monitoring.Shared.DateTimeProviders;

public interface IClock
{
    DateTimeOffset Now { get; }
    DateTimeOffset UtcNow { get; }
}
