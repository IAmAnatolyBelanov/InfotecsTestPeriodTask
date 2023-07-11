namespace Monitoring.Shared.DateTimeProviders;

/// <inheritdoc cref="IClock"/>
public class Clock : IClock
{
    /// <inheritdoc/>
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
