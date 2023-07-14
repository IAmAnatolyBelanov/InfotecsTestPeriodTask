using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monitoring.Dal.Sessions;
using Testcontainers.PostgreSql;

namespace Monitoring.IntegrationTests;

/// <summary>
/// Контейнер для работы интеграционных тестов.
/// </summary>
public class AppFactory : WebApplicationFactory<Monitoring.Api.Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15.3")
        .Build();

    /// <inheritdoc/>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            base.ConfigureWebHost(builder);

            var config = new SessionFactoryConfig
            {
                DbConnectionString = _postgreSqlContainer.GetConnectionString()
            };

            services.RemoveAll<ISessionFactory>();
            services.RemoveAll<ISessionFactoryConfig>();

            services.AddSingleton<ISessionFactoryConfig>(config);
            services.AddSingleton<ISessionFactory>(provider => new SessionFactory(provider.GetRequiredService<ISessionFactoryConfig>()));
        });
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        using var client = CreateClient();

        await client.PostAsync("/Migrations/MigrateUp", null);
    }

    /// <inheritdoc/>
    public new async Task DisposeAsync() => await _postgreSqlContainer.StopAsync();
}
