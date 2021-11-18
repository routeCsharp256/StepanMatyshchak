using System.Threading;
using System.Threading.Tasks;
using Domain.AggregationModels.MerchPack;

namespace Application.Repositories
{
    public interface IMerchPackRepository
    {
        Task Save(MerchPack merchPack, CancellationToken cancellationToken);
        
        Task<MerchPack> GetById(long id, CancellationToken cancellationToken);

        Task<MerchPack> FindByType(MerchPackType merchPackType, CancellationToken cancellationToken);
    }
}