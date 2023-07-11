namespace Monitoring.Shared.DateTimeProviders;

/// <summary>
/// Абстракция над <see cref="DateTimeOffset"/>.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Текущие дата и время по UTC.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}
