using System.Text.Json.Serialization;
using Infotecs.Monitoring.Api.Infrastructure;
using Infotecs.Monitoring.Dal;
using Infotecs.Monitoring.Domain.DeviceBizRules;
using Infotecs.Monitoring.Shared.DateTimeProviders;

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

            builder.RegisterLogger();

            builder.Services.AddTransient<IDeviceService, DeviceService>();

            builder.Services.AddSingleton<IClock, Clock>();

            builder.Services.AddDbContext<MonitoringContext>();
            builder.Services.AddScoped<IMonitoringContext, MonitoringContext>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


            var app = builder.Build();

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
