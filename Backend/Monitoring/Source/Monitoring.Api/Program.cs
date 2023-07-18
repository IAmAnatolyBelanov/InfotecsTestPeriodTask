using Monitoring.Api.Infrastructure;
using Monitoring.Dal;
using Monitoring.Domain.DeviceEventServices;
using Monitoring.Domain.DeviceServices;
using Monitoring.Domain.Mappers;
using Monitoring.Domain.Migrations;
using Monitoring.Shared.DateTimeProviders;
using Monitoring.Shared.GuidProviders;

namespace Monitoring.Api
{
    /// <summary>
    /// Входная точка приложения.
    /// </summary>
    public class Program
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

            builder.Services.SetupDbConnection(builder.Configuration);

            builder.Services.AddFluentMigrator();

            builder.RegisterLogger();

            builder.Services.AddSingleton<IDeviceService, DeviceService>();
            builder.Services.AddSingleton<IDeviceEventService, DeviceEventService>();

            builder.Services.AddSingleton<IClock, Clock>();
            builder.Services.AddSingleton<IGuidProvider, GuidProvider>();

            builder.Services.AddSingleton<IDeviceInfoMapper, DeviceInfoMapper>();
            builder.Services.AddSingleton<IDeviceEventMapper, DeviceEventMapper>();
            builder.Services.AddSingleton<IDeviceEventLightMapper, DeviceEventLightMapper>();

            builder.Services.RegisterControllers();

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

            app.Services.RunMigrations();

            app.Run();
        }
    }
}
