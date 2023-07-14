using FluentMigrator.Runner;
using Monitoring.Dal.Migrations;
using Monitoring.Dal.Sessions;
using Microsoft.Extensions.DependencyInjection;

namespace Monitoring.Domain.Migrations;

/// <summary>
/// Расширения для мигратора.
/// </summary>
public static class MigratorExtensions
{
    /// <summary>
    /// Регистрирует мигратор.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    public static void AddFluentMigrator(this IServiceCollection services)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(x => x.GetRequiredService<ISessionFactoryConfig>().DbConnectionString)
                .ScanIn(typeof(InitialMigration).Assembly).For.Migrations());

        services.AddTransient<IMigrator, Migrator>();
    }

    /// <summary>
    /// Запускает миграции.
    /// </summary>
    /// <param name="services"><see cref="IServiceProvider"/>.</param>
    public static void RunMigrations(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<IMigrator>()
                .MigrateUp();
        }
    }
}
