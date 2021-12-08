using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Integration;
using Application.Repositories;
using MediatR;

namespace Application.Commands.NewMerchandiseAppeared
{
    public class NewMerchandiseAppearedCommand : IRequest<NewMerchandiseAppearedResponse>
    {
        public IReadOnlyCollection<long> SkuCollection { get; init; }
    }

    public class
        NewMerchandiseAppearedCommandHandler : IRequestHandler<NewMerchandiseAppearedCommand, NewMerchandiseAppearedResponse>
    {
        private readonly IMerchandiseRequestRepository _merchandiseRequestRepository;
        private readonly IStockApiIntegration _stockApiIntegration;

        public NewMerchandiseAppearedCommandHandler(IMerchandiseRequestRepository merchandiseRequestRepository,
            IStockApiIntegration stockApiIntegration)
        {
            _merchandiseRequestRepository = merchandiseRequestRepository;
            _stockApiIntegration = stockApiIntegration;
        }

        public async Task<NewMerchandiseAppearedResponse> Handle(NewMerchandiseAppearedCommand request, CancellationToken cancellationToken)
        {
            var allProcessingRequests = await _merchandiseRequestRepository.GetAllProcessingRequests(cancellationToken);

            allProcessingRequests = allProcessingRequests
                .Where(it => it.MerchPack.SkuCollection.Any(sku => request.SkuCollection.Contains(sku.Value)))
                .OrderBy(it => it.CreatedAt)
                .ToArray();

            foreach (var processingRequest in allProcessingRequests)
            {
                var isAvailable = await _stockApiIntegration.RequestGiveOut(
                    processingRequest.MerchPack.SkuCollection.Select(sku => sku.Value), cancellationToken);
                
                processingRequest.GiveOut(isAvailable, DateTimeOffset.Now);
            }

            //TODO
            return new NewMerchandiseAppearedResponse();
        }
    }
}