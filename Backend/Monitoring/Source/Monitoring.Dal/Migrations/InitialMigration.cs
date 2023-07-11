using FluentMigrator;

namespace Infotecs.Monitoring.Dal.Migrations;

/// <summary>
/// Первичная миграция.
/// </summary>
[Migration(202307111545)]
public class InitialMigration : Migration
{
    /// <inheritdoc/>
    public override void Up()
    {
        Create.Table("devices")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("user_name").AsString().NotNullable()
            .WithColumn("operation_system_type").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("operation_system_info").AsString().NotNullable()
            .WithColumn("app_version").AsString().NotNullable()
            .WithColumn("last_update").AsDateTimeOffset().NotNullable().WithDefaultValue("NOW()");
    }

    /// <inheritdoc/>
    public override void Down()
    {
        Delete.Table("devices");
    }
}
