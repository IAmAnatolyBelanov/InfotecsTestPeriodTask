namespace Monitoring.Dal.Sessions;

public class SessionFactory : ISessionFactory
{
    private readonly ISessionFactoryConfig _config;

    public SessionFactory(ISessionFactoryConfig config)
    {
        _config = config;
    }

    public IPgSession CreateForPostgres(bool beginTransaction = false)
    {
        var session = new PgSession(_config.PgConnectionString, beginTransaction);
        return session;
    }
}
