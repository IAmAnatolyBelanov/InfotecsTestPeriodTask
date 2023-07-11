namespace Infotecs.Monitoring.Domain.Migrations;

/// <summary>
/// Мигратор. Управляет миграциями в проекте.
/// </summary>
public interface IMigrator
{
    /// <summary>
    /// Накатывает все найденные и ещё не накаченные миграции.
    /// </summary>
    void MigrateUp();
}
