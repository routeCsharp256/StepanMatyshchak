using FluentMigrator;

namespace Migrator.Temp
{
    [Migration(6)]
    public class FillDictionaries:ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO clothing_sizes (id, name)
                VALUES 
                    (1, 'XS'),
                    (2, 'S'),
                    (3, 'M'),
                    (4, 'L'),
                    (5, 'XL'),
                    (6, 'XXL')
                ON CONFLICT DO NOTHING
            ");
            
            Execute.Sql(@"
                INSERT INTO merch_pack_types (id, name)
                VALUES 
                    (1, 'WelcomePack'),
                    (2, 'StarterPack'),
                    (3, 'ConferenceListenerPack'),
                    (4, 'ConferenceSpeakerPack'),
                    (5, 'VeteranPack')
                ON CONFLICT DO NOTHING
            ");
            
            Execute.Sql(@"
                INSERT INTO request_statuses (id, name)
                VALUES 
                    (1, 'new'),
                    (2, 'processing'),
                    (3, 'done'),
                    (4, 'declined')
                ON CONFLICT DO NOTHING
            ");
        }
    }
}