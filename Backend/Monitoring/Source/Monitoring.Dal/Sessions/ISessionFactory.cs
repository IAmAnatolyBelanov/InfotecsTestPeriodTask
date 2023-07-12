namespace Monitoring.Dal.Sessions;

/// <summary>
/// Фабрика создания сессий.
/// </summary>
public interface ISessionFactory
{
    /// <summary>
    /// Создаёт сессию.
    /// </summary>
    /// <param name="beginTransaction">Необходимо ли открывать транзакцию.</param>
    /// <returns>Новая сессия для работы с БД.</returns>
    ISession CreateSession(bool beginTransaction = false);
}
