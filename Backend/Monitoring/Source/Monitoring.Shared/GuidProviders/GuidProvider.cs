namespace Monitoring.Shared.GuidProviders;

/// <inheritdoc cref="IGuidProvider"/>.
public class GuidProvider : IGuidProvider
{
    /// <inheritdoc/>.
    public Guid NewGuid() => Guid.NewGuid();
}
