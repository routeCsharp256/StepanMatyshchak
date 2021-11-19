using FluentMigrator;

namespace Migrator.Temp
{
    [Migration(3)]
    public class ItemTypes:Migration
    {
        public override void Up()
        {
            Create
                .Table("merch_pack_types")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("merch_pack_types");
        }
    }
}