using System.Text.Json.Serialization;

namespace Monitoring.Api.Infrastructure;

/// <summary>
/// Регистратор контроллеров.
/// </summary>
internal static class ControllersRegister
{
    /// <summary>
    /// Регистрирует контроллеры.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    public static void RegisterControllers(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    }
}
