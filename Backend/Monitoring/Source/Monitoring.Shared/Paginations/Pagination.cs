namespace Infotecs.Monitoring.Shared.Paginations;
public record Pagination
{
    /// <summary>
    /// Индекс страницы. Отсчёт начинается с 0.
    /// </summary>
    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}
