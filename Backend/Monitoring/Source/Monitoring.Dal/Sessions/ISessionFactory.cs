namespace Monitoring.Dal.Sessions;

/// <summary>
/// Фабрика создания сессий.
/// </summary>
public interface ISessionFactory
{
    /// <summary>
    /// Создаёт сессию для работы с PostgreSQL.
    /// </summary>
    /// <param name="beginTransaction">Необходимо ли открывать транзакцию.</param>
    /// <returns>Новая сессия для работы с БД PostgreSQL.</returns>
    IPgSession CreateForPostgres(bool beginTransaction = false);
}
