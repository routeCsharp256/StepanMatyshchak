using System.Threading;
using System.Threading.Tasks;
using Domain.AggregationModels.MerchandiseRequest;

namespace Application.Repositories
{
    public interface IMerchPackRepository
    {
        Task<MerchPack> Create(MerchPack itemToCreate, CancellationToken cancellationToken);
        
        Task<MerchPack> Update(MerchPack itemToUpdate, CancellationToken cancellationToken);
        
        Task<MerchPack> GetById(long id, CancellationToken cancellationToken);

        Task<MerchPack> FindByTypeAndSize(MerchPackType merchPackType, ClothingSize clothingSize, CancellationToken cancellationToken);
    }
}