namespace Monitoring.Dal.Sessions;

/// <inheritdoc cref="ISessionFactory"/>
public class SessionFactory : ISessionFactory
{
    private readonly ISessionFactoryConfig _config;

    /// <summary>
    /// Конструктор класса <see cref="SessionFactory"/>.
    /// </summary>
    /// <param name="config">Конфигурация.</param>
    public SessionFactory(ISessionFactoryConfig config)
    {
        _config = config;
    }

    /// <inheritdoc/>
    public ISession CreateSession(bool beginTransaction = false)
    {
        var session = new Session(_config.DbConnectionString, beginTransaction);
        return session;
    }
}
