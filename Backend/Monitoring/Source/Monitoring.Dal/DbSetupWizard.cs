using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monitoring.Dal.Repositories;
using Monitoring.Dal.Sessions;

namespace Monitoring.Dal;

/// <summary>
/// Мастер настройки подключения к БД.
/// </summary>
public static class DbSetupWizard
{
    /// <summary>
    /// Настраивает работу с БД.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    public static void SetupDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.Configure<SessionFactoryConfig>(configuration.GetSection(SessionFactoryConfig.Position));
        services.AddSingleton<ISessionFactoryConfig>(provider =>
            provider.GetRequiredService<IOptions<SessionFactoryConfig>>().Value);

        services.AddSingleton<ISessionFactory, SessionFactory>();

        services.AddSingleton<IDeviceRepository, DeviceRepository>();
        services.AddSingleton<IEventRepository, EventRepository>();
    }
}
