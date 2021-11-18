using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.AggregationModels.MerchandiseRequest;

namespace Application.Repositories
{
    public interface IMerchandiseRequestRepository
    {
        Task<int> Create(MerchandiseRequest merchandiseRequest, CancellationToken cancellationToken);

        Task Update(MerchandiseRequest merchandiseRequest, CancellationToken cancellationToken);
        
        Task<MerchandiseRequest> GetById(int id, CancellationToken cancellationToken);
        
        Task<IReadOnlyCollection<MerchandiseRequest>> GetByEmployeeEmail(Email email,
            CancellationToken cancellationToken);
        
        Task<IReadOnlyCollection<MerchandiseRequest>> GetAllProcessingRequests(CancellationToken cancellationToken);
    }
}