namespace Infotecs.Monitoring.Shared.Exceptions;

/// <summary>
/// Исключение, которое можно показывать клиенту.
/// </summary>
public class ServerException : Exception
{
    /// <summary>
    /// Конструктор класса <see cref="ServerException"/>.
    /// </summary>
    public ServerException()
        : base()
    {
    }

    /// <summary>
    /// Конструктор класса <see cref="ServerException"/>.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public ServerException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Конструктор класса <see cref="ServerException"/>.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Внутреннее исключение.</param>
    public ServerException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
