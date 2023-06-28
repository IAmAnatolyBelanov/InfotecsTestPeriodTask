using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Infotecs.Monitoring.Bll.DeviceBizRules;
using Infotecs.Monitoring.Bll.LoginBizRules;
using Microsoft.Extensions.Configuration;
using Monitoring.Api;
using Monitoring.Dal;
using Serilog;
using Serilog.Events;

namespace Infotecs.Monitoring.Api
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    path: $"C:\\Logs\\{Assembly.GetExecutingAssembly().GetName().Name}\\log_.log",
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    fileSizeLimitBytes: 1024 * 1024 * 100,
                    rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .CreateLogger();

            builder.Services.AddSingleton<Serilog.ILogger>(logger);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<MonitoringContext>();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddTransient<IDeviceBizRule, DeviceBizRule>();
            builder.Services.AddTransient<ILoginBizRule, LoginBizRule>();

            builder.Host.UseSerilog(logger);

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
