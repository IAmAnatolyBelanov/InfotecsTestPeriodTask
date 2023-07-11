namespace Infotecs.Monitoring.Dal.Sessions;

/// <inheritdoc cref="ISessionFactoryConfig"/>
public class SessionFactoryConfig : ISessionFactoryConfig
{
    /// <summary>
    /// Позиция секции в файле конфигурации.
    /// </summary>
    public const string Position = "DbConnections";

    /// <inheritdoc/>
    public string PgConnectionString { get; set; } = default!;
}
