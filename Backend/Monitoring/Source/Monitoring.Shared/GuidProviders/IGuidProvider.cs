namespace Monitoring.Shared.GuidProviders;

/// <summary>
/// Абстракция над <see cref="Guid"/>.
/// </summary>
public interface IGuidProvider
{
    /// <summary>
    /// Возвращает новый экземпляр <see cref="Guid"/>.
    /// </summary>
    Guid NewGuid();
}
