using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monitoring.Dal.Repositories;
using Monitoring.Dal.Sessions;

namespace Monitoring.Dal;

public static class DbSetupWizard
{
    public static void SetupDbConnection(IServiceCollection services, IConfiguration configuration)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.Configure<SessionFactoryConfig>(configuration.GetSection(SessionFactoryConfig.Position));
        services.AddSingleton<ISessionFactoryConfig>(provider =>
            provider.GetRequiredService<IOptions<SessionFactoryConfig>>().Value);

        services.AddSingleton<ISessionFactory, SessionFactory>();

        services.AddSingleton<IDeviceRepository, DeviceRepository>();
    }
}
