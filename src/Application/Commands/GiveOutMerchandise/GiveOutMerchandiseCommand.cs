using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Integration;
using Application.Repositories;
using Domain.AggregationModels.MerchandiseRequest;
using MediatR;

namespace Application.Commands.GiveOutMerchandise
{
    public record GiveOutMerchandiseCommand : IRequest<GiveOutMerchandiseResponse>
    {
        public string Email { get; init; }
        
        public string ClothingSize { get; init; }

        public string Type { get; init; }
    }
    
    public class GiveOutMerchandiseCommandHandler : IRequestHandler<GiveOutMerchandiseCommand, GiveOutMerchandiseResponse>
    {
        private readonly IMerchandiseRequestRepository _merchandiseRequestRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IStockApiIntegration _stockApiIntegration;
        private readonly IEmailService _emailService;

        public GiveOutMerchandiseCommandHandler(IMerchandiseRequestRepository merchandiseRequestRepository,
            IMerchPackRepository merchPackRepository,
            IStockApiIntegration stockApiIntegration,
            IEmailService emailService)
        {
            _merchandiseRequestRepository = merchandiseRequestRepository;
            _merchPackRepository = merchPackRepository;
            _stockApiIntegration = stockApiIntegration;
            _emailService = emailService;
        }

        public async Task<GiveOutMerchandiseResponse> Handle(GiveOutMerchandiseCommand request, CancellationToken cancellationToken)
        {
            var merchPack = await _merchPackRepository.FindByTypeAndSize(MerchPackType.Parse(request.Type),
                ClothingSize.Parse(request.ClothingSize),
                cancellationToken);

            var existingRequests =
                await _merchandiseRequestRepository.GetByEmployeeEmail(Email.Create(request.Email), cancellationToken);

            var newMerchandiseRequest = MerchandiseRequest.Create(
                merchPack: merchPack,
                employee: new Employee(Email.Create(request.Email), ClothingSize.Parse(request.ClothingSize)),
                existingRequests: existingRequests,
                createdAt: DateTimeOffset.Now
            );

            var createdRequest = await _merchandiseRequestRepository.Create(newMerchandiseRequest, cancellationToken);
            var newId = createdRequest.Id;

            newMerchandiseRequest = new MerchandiseRequest(
                newId,
                newMerchandiseRequest.MerchPack,
                newMerchandiseRequest.Employee,
                newMerchandiseRequest.Status,
                newMerchandiseRequest.CreatedAt,
                newMerchandiseRequest.GaveOutAt);

            var isSkuPackAvailable =
                await _stockApiIntegration.RequestGiveOut(merchPack.SkuCollection.Select(sku => sku.Value),
                    cancellationToken);
            
            newMerchandiseRequest.GiveOut(isSkuPackAvailable, DateTimeOffset.UtcNow);

            await _merchandiseRequestRepository.Update(newMerchandiseRequest, cancellationToken);

            await _emailService.SendEmail(newMerchandiseRequest.Employee.Email, new object());

            //TODO
            return new GiveOutMerchandiseResponse(); 
        }
    }
}