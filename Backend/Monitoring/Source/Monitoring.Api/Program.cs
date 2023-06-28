using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Infotecs.Monitoring.Bll.DeviceBizRules;
using Infotecs.Monitoring.Bll.LoginBizRules;
using Microsoft.Extensions.Configuration;
using Infotecs.Monitoring.Api;
using Infotecs.Monitoring.Dal;
using Infotecs.Monitoring.Shared.DateTimeProviders;
using Serilog;
using Serilog.Events;
using Infotecs.Monitoring.Api.Infrastructure;

namespace Infotecs.Monitoring.Api
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            LoggerRegister.RegisterLogger(builder);

            builder.Services.AddTransient<IDeviceBizRule, DeviceBizRule>();
            builder.Services.AddTransient<ILoginBizRule, LoginBizRule>();

            builder.Services.AddSingleton<IClock, Clock>();

            builder.Services.AddDbContext<MonitoringContext>();


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
