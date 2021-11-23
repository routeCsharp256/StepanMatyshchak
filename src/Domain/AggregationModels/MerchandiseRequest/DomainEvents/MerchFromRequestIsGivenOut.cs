using MediatR;

namespace Domain.AggregationModels.MerchandiseRequest.DomainEvents
{
    public sealed record MerchFromRequestIsGivenOut : INotification
    {
        public Employee Employee { get; init; }
        
        public MerchPack MerchPack { get; init; }
    }
}