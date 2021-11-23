using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Dapper;
using Npgsql;
using Domain.AggregationModels.MerchandiseRequest;
using Infrastructure.Repositories.Dtos;
using Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Infrastructure.Repositories.Implementation
{
    public class MerchPackRepository : IMerchPackRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IQueryExecutor _queryExecutor;
        private const int Timeout = 5;

        public MerchPackRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IQueryExecutor queryExecutor)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _queryExecutor = queryExecutor;
        }

        public async Task<MerchPack> Create(MerchPack itemToCreate, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO merch_packs (merch_pack_type_id, clothing_size_id, skus)
                VALUES (@MerchPackTypeId, @ClothingSizeId, @Skus) RETURNING id;";

            var parameters = new
            {
                MerchPackTypeId = itemToCreate.MerchPackType.Id,
                ClothingSizeId = itemToCreate.ClothingSize.Id,
                Skus = itemToCreate.SkuCollection
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            
            return await _queryExecutor.Execute(async () =>
            {
                var newId = await connection.QueryFirstAsync<long>(commandDefinition);

                var entityWithId = new MerchPack(
                    newId,
                    itemToCreate.MerchPackType,
                    itemToCreate.ClothingSize,
                    itemToCreate.SkuCollection);

                return entityWithId;
            });
        }

        public async Task<MerchPack> Update(MerchPack itemToUpdate, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE merch_packs
                SET merch_pack_type_id = @MerchPackTypeId, clothing_size_id = @ClothingSizeId, skus = @Skus
                WHERE id = @MerchPackId;";
            
            var parameters = new
            {
                MerchPackId = itemToUpdate.Id,
                MerchPackTypeId = itemToUpdate.MerchPackType.Id,
                ClothingSizeId = itemToUpdate.ClothingSize.Id,
                Skus = itemToUpdate.SkuCollection,
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            
            return await _queryExecutor.Execute(itemToUpdate, () => connection.ExecuteAsync(commandDefinition));
        }

        public async Task<MerchPack> GetById(long id, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT merch_packs.id, merch_packs.merch_pack_type_id, merch_packs.clothing_size_id, merch_packs.skus,
                       merch_pack_types.id, merch_pack_types.name,
                       clothing_sizes.id, clothing_sizes.name
                FROM merch_packs
                INNER JOIN merch_pack_types on merch_pack_types.id = merch_packs.merch_pack_type_id
                LEFT JOIN clothing_sizes on clothing_sizes.id = merch_packs.clothing_size_id
                WHERE merch_packs.id = @MerchPackId;";
            
            var parameters = new
            {
                MerchPackId = id,
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            
            return await _queryExecutor.Execute(
                async () =>
                {
                    var merchPacks = await connection.QueryAsync<
                        MerchPackDto, MerchPackTypeDto, ClothingSizeDto, MerchPack>(
                        commandDefinition,
                        (merchPackModel, merchPackType, clothingSize) => new MerchPack(
                            merchPackModel.Id, MerchPackType.Parse(merchPackType.Name), ClothingSize.Parse(clothingSize.Name),
                            merchPackModel.SkuCollection.Select(Sku.Create).ToList().AsReadOnly()
                        ));
                    
                    return merchPacks.First();
                });
        }

        public async Task<MerchPack> FindByTypeAndSize(MerchPackType merchPackType, ClothingSize clothingSize, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT merch_packs.id, merch_packs.merch_pack_type_id, merch_packs.clothing_size_id, merch_packs.skus,
                       merch_pack_types.id, merch_pack_types.name,
                       clothing_sizes.id, clothing_sizes.name
                FROM merch_packs
                INNER JOIN merch_pack_types on merch_pack_types.id = merch_packs.merch_pack_type_id
                LEFT JOIN clothing_sizes on clothing_sizes.id = merch_packs.clothing_size_id
                WHERE merch_packs.merch_pack_type_id = @MerchPackTypeId AND merch_packs.clothing_size_id = @ClothingSizeId;";
            
            var parameters = new
            {
                MerchPackTypeId = merchPackType.Id,
                ClothingSizeId = clothingSize.Id
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            
            return await _queryExecutor.Execute(
                async () =>
                {
                    var merchPacks = await connection.QueryAsync<
                        MerchPackDto, MerchPackTypeDto, ClothingSizeDto, MerchPack>(
                        commandDefinition,
                        (merchPackDto, merchPackTypeDto, clothingSizeDto) => new MerchPack(
                            merchPackDto.Id, MerchPackType.Parse(merchPackTypeDto.Name), ClothingSize.Parse(clothingSizeDto.Name),
                            merchPackDto.SkuCollection.Select(Sku.Create).ToList().AsReadOnly()
                        ));
                    
                    return merchPacks.First();
                });
        }
    }
}