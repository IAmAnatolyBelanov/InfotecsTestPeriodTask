namespace Monitoring.Dal.Sessions;

public class SessionFactoryConfig : ISessionFactoryConfig
{
    public const string Position = "DbConnections";

    public string PgConnectionString { get; set; } = default!;
}
