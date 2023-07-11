namespace Monitoring.Dal.Sessions;

public class SessionFactoryConfig : ISessionFactoryConfig
{
    public string PgConnectionString { get; set; } = "User ID=postgres;Password=mysecretpassword;Host=localhost;Port=5432;Database=testperiod;Pooling=true;";
}
