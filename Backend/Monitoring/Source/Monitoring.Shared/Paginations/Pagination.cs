namespace Infotecs.Monitoring.Shared.Paginations;

/// <summary>
/// Пагинация.
/// </summary>
public record Pagination
{
    /// <summary>
    /// Индекс страницы.
    /// </summary>
    /// <remarks>Отсчёт начинается с 0.</remarks>
    public int PageIndex { get; init; }

    /// <summary>
    /// Размер страницы.
    /// </summary>
    public int PageSize { get; init; }
}
