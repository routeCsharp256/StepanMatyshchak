using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain.AggregationModels.MerchPack;
using Infrastructure.Repositories.Infrastructure.Interfaces;
using Npgsql;

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

        public Task Save(MerchPack merchPack, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<MerchPack> GetById(long id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<MerchPack> FindByType(MerchPackType merchPackType, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}