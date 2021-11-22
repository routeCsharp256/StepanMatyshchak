using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Dapper;
using Domain.AggregationModels.MerchandiseRequest;
using Domain.AggregationModels.MerchPack;
using Infrastructure.Repositories.Dtos;
using Infrastructure.Repositories.Infrastructure.Interfaces;
using Npgsql;
using MerchandiseRequestDto = Application.Queries.GetRequestsByEmployee.MerchandiseRequestDto;

namespace Infrastructure.Repositories.Implementation
{
    public class MerchandiseRequestRepository : IMerchandiseRequestRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IQueryExecutor _queryExecutor;
        private const int Timeout = 5;

        public MerchandiseRequestRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IQueryExecutor queryExecutor)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _queryExecutor = queryExecutor;
        }

        public async Task<MerchandiseRequest> Create(MerchandiseRequest itemToCreate,
            CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO merchandise_requests (merch_pack_id, employee_email, employee_clothing_size_id, request_status_id, created_at, gave_out_at)
                VALUES (@MerchPackId, @EmployeeEmail, @EmployeeClothingSizeId, @RequestStatusId, @CreatedAt, @GaveOutAt);";

            var parameters = new
            {
                MerchPackId = itemToCreate.MerchPack.Id,
                EmployeeEmail = itemToCreate.Employee.Email.Value,
                EmployeeClothingSizeId = itemToCreate.Employee.ClothingSize.Id,
                RequestStatusId = itemToCreate.Status.Id,
                CreatedAt = itemToCreate.CreatedAt,
                GaveOutAt = itemToCreate.GaveOutAt,
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            return await _queryExecutor.Execute(itemToCreate, () => connection.ExecuteAsync(commandDefinition));        }

        public async Task<MerchandiseRequest> Update(MerchandiseRequest itemToUpdate,
            CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE merchandise_requests
                SET merch_pack_id = @MerchPackId,
                    employee_email = @EmployeeEmail,
                    employee_clothing_size_id = @EmployeeClothingSizeId,
                    request_status_id = @RequestStatusId,
                    created_at = @CreatedAt,
                    gave_out_at = @GaveOutAt
                WHERE id = @MerchandiseRequestId;";
            
            var parameters = new
            {
                MerchandiseRequestId = itemToUpdate.Id,
                MerchPackId = itemToUpdate.MerchPack.Id,
                EmployeeEmail = itemToUpdate.Employee.Email.Value,
                EmployeeClothingSizeId = itemToUpdate.Employee.ClothingSize.Id,
                RequestStatusId = itemToUpdate.Status.Id,
                CreatedAt = itemToUpdate.CreatedAt,
                GaveOutAt = itemToUpdate.GaveOutAt
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            return await _queryExecutor.Execute(itemToUpdate, () => connection.ExecuteAsync(commandDefinition));
        }

        public async Task<MerchandiseRequest> GetById(int id, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT mr.id, mr.merch_pack_id, mr.employee_email, mr.employee_clothing_size_id, mr.request_status_id, mr.created_at, mr.gave_out_at,
                       request_statuses.id, request_statuses.name,
                       mp.id, mp.merch_pack_type_id, mp.clothing_size_id, mp.skus,
                       merch_pack_types.id, merch_pack_types.name,
                       clothing_sizes.id, clothing_sizes.name
                FROM merchandise_requests as mr
                INNER JOIN request_statuses as rs on rs.id = mr.request_status_id
                INNER JOIN merch_packs as mp on mp.id = mr.merch_pack_id
                INNER JOIN merch_pack_types as mpt on mpt.id = mp.merch_pack_type_id
                LEFT JOIN clothing_sizes as cs on cs.id = mp.clothing_size_id
                WHERE merchandise_requests.id = @MerchandiseRequestId;";
            
            var parameters = new
            {
                MerchandiseRequestId = id,
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
                        Repositories.Dtos.MerchandiseRequestDto, RequestStatusDto, MerchPackDto, MerchPackTypeDto, ClothingSizeDto, MerchandiseRequest>(
                        commandDefinition,
                        (merchandiseRequestDto, requestStatusDto, merchPackDto, merchPackTypeDto, clothingSizeDto) => 
                            new MerchandiseRequest(
                            merchandiseRequestDto.Id,
                            new MerchPack(merchPackDto.Id,
                                MerchPackType.Parse(merchPackTypeDto.Name),
                                ClothingSize.Parse(clothingSizeDto.Name),
                                merchPackDto.SkuCollection.Select(Sku.Create).ToList().AsReadOnly()),
                            new Employee(Email.Create(merchandiseRequestDto.EmployeeEmail), ClothingSize.Parse(clothingSizeDto.Name)),
                            MerchandiseRequestStatus.Parse(requestStatusDto.Name),
                            merchandiseRequestDto.CreatedAt,
                            merchandiseRequestDto.GaveOutAt
                        ));
                    
                    return merchPacks.First();
                });
        }

        public async Task<IReadOnlyCollection<MerchandiseRequest>> GetByEmployeeEmail(Email email, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT mr.id, mr.merch_pack_id, mr.employee_email, mr.employee_clothing_size_id, mr.request_status_id, mr.created_at, mr.gave_out_at,
                       request_statuses.id, request_statuses.name,
                       mp.id, mp.merch_pack_type_id, mp.clothing_size_id, mp.skus,
                       merch_pack_types.id, merch_pack_types.name,
                       clothing_sizes.id, clothing_sizes.name
                FROM merchandise_requests as mr
                INNER JOIN request_statuses as rs on rs.id = mr.request_status_id
                INNER JOIN merch_packs as mp on mp.id = mr.merch_pack_id
                INNER JOIN merch_pack_types as mpt on mpt.id = mp.merch_pack_type_id
                LEFT JOIN clothing_sizes as cs on cs.id = mp.clothing_size_id
                WHERE merchandise_requests.employee_email = @EmployeeEmail;";
            
            var parameters = new
            {
                EmployeeEmail = email.Value
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var result = await _queryExecutor.Execute(
                async () =>
                {
                    var merchPacks = await connection.QueryAsync<
                        Repositories.Dtos.MerchandiseRequestDto, RequestStatusDto, MerchPackDto, MerchPackTypeDto,
                        ClothingSizeDto, MerchandiseRequest>(
                        commandDefinition,
                        (merchandiseRequestDto, requestStatusDto, merchPackDto, merchPackTypeDto, clothingSizeDto) =>
                            new MerchandiseRequest(
                                merchandiseRequestDto.Id,
                                new MerchPack(merchPackDto.Id,
                                    MerchPackType.Parse(merchPackTypeDto.Name),
                                    ClothingSize.Parse(clothingSizeDto.Name),
                                    merchPackDto.SkuCollection.Select(Sku.Create).ToList().AsReadOnly()),
                                new Employee(Email.Create(merchandiseRequestDto.EmployeeEmail),
                                    ClothingSize.Parse(clothingSizeDto.Name)),
                                MerchandiseRequestStatus.Parse(requestStatusDto.Name),
                                merchandiseRequestDto.CreatedAt,
                                merchandiseRequestDto.GaveOutAt
                            ));

                    return merchPacks;
                });

            return result.ToList();
        }

        public async Task<IReadOnlyCollection<MerchandiseRequest>> GetAllProcessingRequests(CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT mr.id, mr.merch_pack_id, mr.employee_email, mr.employee_clothing_size_id, mr.request_status_id, mr.created_at, mr.gave_out_at,
                       request_statuses.id, request_statuses.name,
                       mp.id, mp.merch_pack_type_id, mp.clothing_size_id, mp.skus,
                       merch_pack_types.id, merch_pack_types.name,
                       clothing_sizes.id, clothing_sizes.name
                FROM merchandise_requests as mr
                INNER JOIN request_statuses as rs on rs.id = mr.request_status_id
                INNER JOIN merch_packs as mp on mp.id = mr.merch_pack_id
                INNER JOIN merch_pack_types as mpt on mpt.id = mp.merch_pack_type_id
                LEFT JOIN clothing_sizes as cs on cs.id = mp.clothing_size_id
                WHERE request_status_id = @RequestStatusId;";
            
            var parameters = new
            {
                RequestStatusId = MerchandiseRequestStatus.Processing.Id
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var result = await _queryExecutor.Execute(
                async () =>
                {
                    var merchPacks = await connection.QueryAsync<
                        Repositories.Dtos.MerchandiseRequestDto, RequestStatusDto, MerchPackDto, MerchPackTypeDto,
                        ClothingSizeDto, MerchandiseRequest>(
                        commandDefinition,
                        (merchandiseRequestDto, requestStatusDto, merchPackDto, merchPackTypeDto, clothingSizeDto) =>
                            new MerchandiseRequest(
                                merchandiseRequestDto.Id,
                                new MerchPack(merchPackDto.Id,
                                    MerchPackType.Parse(merchPackTypeDto.Name),
                                    ClothingSize.Parse(clothingSizeDto.Name),
                                    merchPackDto.SkuCollection.Select(Sku.Create).ToList().AsReadOnly()),
                                new Employee(Email.Create(merchandiseRequestDto.EmployeeEmail),
                                    ClothingSize.Parse(clothingSizeDto.Name)),
                                MerchandiseRequestStatus.Parse(requestStatusDto.Name),
                                merchandiseRequestDto.CreatedAt,
                                merchandiseRequestDto.GaveOutAt
                            ));

                    return merchPacks;
                });

            return result.ToList();
        }
    }
}