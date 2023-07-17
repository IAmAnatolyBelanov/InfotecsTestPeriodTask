using FluentMigrator;

namespace Monitoring.Dal.Migrations;

/// <summary>
/// Миграция с добавлением событий.
/// </summary>
[Migration(202307121450)]
public class AddEvents : Migration
{
    /// <inheritdoc/>
    public override void Up()
    {
        Create.Table("events")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("device_id").AsGuid().NotNullable().ForeignKey("devices", "id")
            .WithColumn("name").AsString(50).NotNullable()
            .WithColumn("date_time").AsDateTimeOffset().NotNullable();

        Create.Index()
            .OnTable("events")
            .OnColumn("device_id");
    }

    /// <inheritdoc/>
    public override void Down()
    {
        Delete.Table("events");
    }
}
