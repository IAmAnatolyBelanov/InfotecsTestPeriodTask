namespace Monitoring.Dal.Sessions;

public interface ISessionFactory
{
    IPgSession CreateForPostgres(bool beginTransaction = false);
}
