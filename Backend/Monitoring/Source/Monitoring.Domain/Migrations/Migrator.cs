using FluentMigrator.Runner;

namespace Infotecs.Monitoring.Domain.Migrations;

/// <inheritdoc cref="IMigrator"/>
public class Migrator : IMigrator
{
    private readonly IMigrationRunner _migrationRunner;

    /// <summary>
    /// Конструктор класса <see cref="Migrator"/>.
    /// </summary>
    /// <param name="migrationRunner"><see cref="IMigrationRunner"/>.</param>
    public Migrator(IMigrationRunner migrationRunner)
    {
        _migrationRunner = migrationRunner;
    }

    /// <inheritdoc/>
    public void MigrateUp()
    {
        _migrationRunner.MigrateUp();
    }
}
