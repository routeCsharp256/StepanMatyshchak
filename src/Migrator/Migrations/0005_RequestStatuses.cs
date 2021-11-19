using FluentMigrator;

namespace Migrator.Temp
{
    [Migration(5)]
    public class RequestStatuses:Migration
    {
        public override void Up()
        {
            Create
                .Table("request_statuses")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("request_statuses");
        }
    }
}