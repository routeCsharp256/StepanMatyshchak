using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain.AggregationModels.MerchandiseRequest;
using MediatR;

namespace Application.Queries.GetRequestsByEmployee
{
    public class GetRequestsByEmployeeQuery : IRequest<GetRequestsByEmployeeQueryResponse>
    {
        public string Email { get; init; }
    }
    
    public class GetRequestsByEmployeeQueryHandler :
        IRequestHandler<GetRequestsByEmployeeQuery, GetRequestsByEmployeeQueryResponse>
    {
        private readonly IMerchandiseRequestRepository _merchandiseRequestRepository;

        public GetRequestsByEmployeeQueryHandler(IMerchandiseRequestRepository merchandiseRequestRepository)
        {
            _merchandiseRequestRepository = merchandiseRequestRepository;
        }

        public async Task<GetRequestsByEmployeeQueryResponse> Handle(GetRequestsByEmployeeQuery request, CancellationToken cancellationToken)
        {
            var requests =
                await _merchandiseRequestRepository.GetByEmployeeEmail(Email.Create(request.Email), cancellationToken);

            return new GetRequestsByEmployeeQueryResponse()
            {
                Items = requests.Select(it => new MerchandiseRequestDto
                {
                    Status = it.Status.Name,
                    Type = it.MerchPack.MerchPackType.Name,
                    CreatedAt = it.CreatedAt,
                    GaveOutAt = it.GaveOutAt
                }).ToArray()
            };
        }
    }
}