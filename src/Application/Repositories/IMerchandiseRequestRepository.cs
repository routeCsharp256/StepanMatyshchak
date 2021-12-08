using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.AggregationModels.MerchandiseRequest;

namespace Application.Repositories
{
    public interface IMerchandiseRequestRepository
    {
        Task<MerchandiseRequest> Create(MerchandiseRequest itemToCreate, CancellationToken cancellationToken);

        Task<MerchandiseRequest> Update(MerchandiseRequest itemToUpdate, CancellationToken cancellationToken);
        
        Task<MerchandiseRequest> GetById(int id, CancellationToken cancellationToken);
        
        Task<IReadOnlyCollection<MerchandiseRequest>> GetByEmployeeEmail(Email email,
            CancellationToken cancellationToken);
        
        Task<IReadOnlyCollection<MerchandiseRequest>> GetAllProcessingRequests(CancellationToken cancellationToken);
    }
}