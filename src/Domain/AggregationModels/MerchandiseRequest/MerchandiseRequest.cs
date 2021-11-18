using System;
using System.Collections.Generic;
using Domain.AggregationModels.MerchandiseRequest.DomainEvents;
using Domain.BaseModels;

namespace Domain.AggregationModels.MerchandiseRequest
{
    public class MerchandiseRequest : Entity, IAggregationRoot
    {
        private readonly MerchPack.MerchPack _merchPack;
        private readonly Employee _employee;
        private readonly DateTimeOffset _createdAt;

        public MerchandiseRequest(MerchPack.MerchPack merchPack,
            Employee employee,
            MerchandiseRequestStatus status,
            DateTimeOffset createdAt,
            DateTimeOffset? gaveOutAt)
        {
            MerchPack = merchPack;
            Employee = employee;
            Status = status;
            CreatedAt = createdAt;
            GaveOutAt = gaveOutAt;
        }

        public MerchPack.MerchPack MerchPack { get; }
        
        public Employee Employee { get; }
        
        public MerchandiseRequestStatus Status { get; private set; }
        
        public DateTimeOffset CreatedAt { get; }
        
        public DateTimeOffset? GaveOutAt { get; private set; }

        private MerchandiseRequest(MerchPack.MerchPack merchPack, Employee employee, DateTimeOffset createdAt)
        {
            _merchPack = merchPack;
            _employee = employee;
            _createdAt = createdAt;
            Status = MerchandiseRequestStatus.New;
        }
        
        public static MerchandiseRequest Create(MerchPack.MerchPack merchPack,
            Employee employee,
            DateTimeOffset createdAt,
            IReadOnlyCollection<MerchandiseRequest> existingRequests)
        {
            var newRequest = new MerchandiseRequest(merchPack, employee, createdAt);
            if (newRequest.CheckAbilityToGiveOut(existingRequests, createdAt))
            {
                return newRequest;
            }            
            
            throw new DomainException("Unable to give out merchandise");
        }

        public void GiveOut(bool isAvailableOnStock, DateTimeOffset gaveOutAt)
        {
            if (Equals(Status, MerchandiseRequestStatus.New) || Equals(Status, MerchandiseRequestStatus.Processing))
            {
                if (isAvailableOnStock)
                {
                    Status = MerchandiseRequestStatus.Done;
                    GaveOutAt = gaveOutAt;
                    
                    AddDomainEvent(new MerchFromRequestIsGivenOut
                    {
                        MerchPack = MerchPack,
                        Employee = Employee,
                    });
                }
                else
                {
                    Status = MerchandiseRequestStatus.Processing;
                }
            }
            else
            {
                throw new DomainException($"Unable to give out merchandise for request in status: {Status}");
            }
        }

        public void Decline()
        {
            if (Equals(Status, MerchandiseRequestStatus.Done) || Equals(Status, MerchandiseRequestStatus.Declined))
            {
                throw new DomainException($"Unable to decline out merchandise for request in status: {Status}");
            }
        }

        private bool CheckAbilityToGiveOut(IReadOnlyCollection<MerchandiseRequest> existingRequests, DateTimeOffset gaveOutAt)
        {
            return true;
        }
    }
}