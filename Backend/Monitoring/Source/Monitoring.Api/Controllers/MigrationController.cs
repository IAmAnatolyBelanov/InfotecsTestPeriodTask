using Infotecs.Monitoring.Api.Infrastructure;
using Infotecs.Monitoring.Domain.Migrations;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.Monitoring.Api.Controllers;

/// <summary>
/// Контроллер для работы с миграциями.
/// </summary>
[ApiController]
[Route("Migrations")]
public class MigrationController
{
    private readonly IMigrator _migrator;

    /// <summary>
    /// Конструктор класса <see cref="MigrationController"/>.
    /// </summary>
    /// <param name="migrator"><see cref="IMigrator"/>.</param>
    public MigrationController(IMigrator migrator)
    {
        _migrator = migrator;
    }

    /// <summary>
    /// Накатывает все найденные и ещё не накаченные миграции.
    /// </summary>
    /// <returns>Пустой <see cref="BaseResponse{T}"/> в случае успешного выполнения миграций, либо ошибки в случае их возникновения.</returns>
    [HttpPost("MigrateUp")]
    public BaseResponse<object> MigrateUp()
    {
        _migrator.MigrateUp();
        return BaseResponseExtensions.EmptySuccess();
    }
}
