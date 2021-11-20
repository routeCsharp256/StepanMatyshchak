using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain.AggregationModels.MerchandiseRequest;
using Infrastructure.Repositories.Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories.Implementation
{
    public class MerchandiseRequestRepository : IMerchandiseRequestRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IQueryExecutor _queryExecutor;

        public MerchandiseRequestRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IQueryExecutor queryExecutor)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _queryExecutor = queryExecutor;
        }

        public Task<int> Create(MerchandiseRequest merchandiseRequest, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(MerchandiseRequest merchandiseRequest, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<MerchandiseRequest> GetById(int id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyCollection<MerchandiseRequest>> GetByEmployeeEmail(Email email, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyCollection<MerchandiseRequest>> GetAllProcessingRequests(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}