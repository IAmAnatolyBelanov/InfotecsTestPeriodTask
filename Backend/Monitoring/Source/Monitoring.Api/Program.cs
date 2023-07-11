using System.Text.Json.Serialization;
using Infotecs.Monitoring.Api.Infrastructure;
using Infotecs.Monitoring.Domain.DeviceBizRules;
using Infotecs.Monitoring.Domain.Mappers;
using Infotecs.Monitoring.Shared.DateTimeProviders;
using Infotecs.Monitoring.Dal.Sessions;
using Microsoft.Extensions.Options;
using Infotecs.Monitoring.Dal.Repositories;
using Infotecs.Monitoring.Domain.Migrations;

namespace Infotecs.Monitoring.Api
{
    /// <summary>
    /// Входная точка приложения.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Запускает сервис.
        /// </summary>
        /// <param name="args">Аргументы для запуска сервиса.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
                .AddEnvironmentVariables();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            builder.Services.Configure<SessionFactoryConfig>(builder.Configuration.GetSection(SessionFactoryConfig.Position));
            builder.Services.AddSingleton<ISessionFactoryConfig>(provider =>
                provider.GetRequiredService<IOptions<SessionFactoryConfig>>().Value);

            builder.Services.RegisterFluentMigrator();

            builder.RegisterLogger();

            builder.Services.AddTransient<IDeviceService, DeviceService>();

            builder.Services.AddSingleton<IClock, Clock>();

            builder.Services.AddSingleton<ISessionFactory, SessionFactory>();

            builder.Services.AddSingleton<IPgDeviceRepository, PgDeviceRepository>();

            builder.Services.AddSingleton<IDeviceInfoMapper, DeviceInfoMapper>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddCors();

            var app = builder.Build();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
