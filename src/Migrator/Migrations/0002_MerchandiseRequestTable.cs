using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Migrations
{
    [Migration(2)]
    public class MerchandiseRequestTable: Migration
    {
        public override void Up()
        {
            Create
                .Table("merchandise_requests")
                .WithColumn("id").AsInt64().Identity().PrimaryKey()
                .WithColumn("merch_pack_id").AsInt64().NotNullable()
                .WithColumn("employee_email").AsString().NotNullable()
                .WithColumn("employee_clothing_size_id").AsInt32().NotNullable()
                .WithColumn("request_status_id").AsInt32().NotNullable()
                .WithColumn("created_at").AsDateTimeOffset().NotNullable()
                .WithColumn("gave_out_at").AsDateTimeOffset();
        }

        public override void Down()
        {
            Delete.Table("merchandise_requests");
        }
    }
}