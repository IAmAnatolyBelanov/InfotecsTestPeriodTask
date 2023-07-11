namespace Monitoring.Dal.Sessions;

public interface ISessionFactoryConfig
{
    string PgConnectionString { get; }
}
