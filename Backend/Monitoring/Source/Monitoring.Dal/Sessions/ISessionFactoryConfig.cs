namespace Infotecs.Monitoring.Dal.Sessions;

/// <summary>
/// Конфигурация для <see cref="ISessionFactory"/>.
/// </summary>
public interface ISessionFactoryConfig
{
    /// <summary>
    /// Строка подключения к БД PostgreSQL.
    /// </summary>
    string PgConnectionString { get; }
}
