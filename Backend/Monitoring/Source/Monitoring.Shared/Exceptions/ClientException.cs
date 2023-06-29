namespace Infotecs.Monitoring.Shared.Exceptions;

/// <summary>
/// Исключение, которое можно показывать клиенту.
/// </summary>
public class ClientException : Exception
{
    /// <summary>
    /// Конструктор класса <see cref="ClientException"/>.
    /// </summary>
    public ClientException() : base() { }

    /// <summary>
    /// Конструктор класса <see cref="ClientException"/>.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public ClientException(string message) : base(message) { }

    /// <summary>
    /// Конструктор класса <see cref="ClientException"/>.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Внутреннее исключение.</param>
    public ClientException(string message, Exception innerException) : base(message, innerException) { }
}
