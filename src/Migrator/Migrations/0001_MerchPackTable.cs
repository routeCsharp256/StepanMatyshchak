using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Migrations
{
    [Migration(1)]
    public class MerchPackTable: Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE if not exists merch_packs(
                    id BIGSERIAL PRIMARY KEY,
                    merch_pack_type_id INT NOT NULL,
                    clothing_size_id INT,
                    skus bigint[]);"
            );
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE if exists merch_packs;");
        }
    }
}