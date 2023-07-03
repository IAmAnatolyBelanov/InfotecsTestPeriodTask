namespace Infotecs.Monitoring.Api.Infrastructure;

/// <summary>
/// Расширения для работы с классом BaseResponse.
/// </summary>
public static class BaseResponseExtensions
{
    /// <summary>
    /// Возвращает BaseResponse, хранящий переданные данные.
    /// </summary>
    /// <typeparam name="T">Тип данных, что будут возвращены в ответ на API запрос.</typeparam>
    /// <param name="data">Данные, что будут возвращены в ответ на API запрос.</param>
    /// <returns>BaseResponse, хранящий переданные данные.</returns>
    public static BaseResponse<T> ToResponse<T>(this T data) => new BaseResponse<T> { Data = data };

    /// <summary>
    /// Возвращает BaseResponse используемый, когда API не должен возвращать никакой информации кроме успешности выполнения запроса.
    /// </summary>
    /// <returns>BaseResponse используемый, когда API не должен возвращать никакой информации кроме успешности выполнения запроса.</returns>
    public static BaseResponse<object> EmptySuccess() => new BaseResponse<object> { Data = null };
}
