namespace Monitoring.Api;

public class BaseResponse<T>
{
    public T? Data { get; init; }
    public string? Error { get; init; }
}

public static class BaseResponseExtensions
{
    public static BaseResponse<T> ToResponse<T>(this T data)
    {
        return new BaseResponse<T> { Data = data };
    }
}
