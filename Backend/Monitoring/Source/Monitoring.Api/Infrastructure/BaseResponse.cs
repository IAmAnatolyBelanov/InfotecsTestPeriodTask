namespace Infotecs.Monitoring.Api.Infrastructure;

/// <summary>
/// Класс-обёртка для ответов, возвращаемых на API запросы.
/// </summary>
/// <typeparam name="T">Тип данных, которые должны быть возвращены в ответ на API запрос.</typeparam>
public class BaseResponse<T>
{
    /// <summary>
    /// Ответ на API запрос.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Ошибка, возникшая в ходе выполнения API запроса.
    /// </summary>
    public string? Error { get; init; }
}
