using FluentMigrator.Runner;
using Monitoring.Dal.Migrations;
using Monitoring.Dal.Sessions;
using Microsoft.Extensions.DependencyInjection;

namespace Monitoring.Domain.Migrations;

/// <summary>
/// Регистратор мигратора.
/// </summary>
public static class MigratorRegister
{
    /// <summary>
    /// Регистрирует мигратор.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    public static void RegisterFluentMigrator(this IServiceCollection services)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(x => x.GetRequiredService<ISessionFactoryConfig>().PgConnectionString)
                .ScanIn(typeof(InitialMigration).Assembly).For.Migrations());

        services.AddTransient<IMigrator, Migrator>();
    }
}
