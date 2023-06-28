using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Infotecs.Monitoring.Bll.DeviceBizRules;
using Infotecs.Monitoring.Bll.LoginBizRules;
using Monitoring.Dal;

namespace Infotecs.Monitoring.Api
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            var app = builder.Build();

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
