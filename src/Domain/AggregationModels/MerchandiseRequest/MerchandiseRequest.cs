using System;
using System.Collections.Generic;
using System.Linq;
using Domain.AggregationModels.MerchandiseRequest.DomainEvents;
using Domain.BaseModels;

namespace Domain.AggregationModels.MerchandiseRequest
{
    public class MerchandiseRequest : Entity, IAggregationRoot
    {
        public MerchandiseRequest(long id,
            MerchPack merchPack,
            Employee employee,
            MerchandiseRequestStatus status,
            DateTimeOffset createdAt,
            DateTimeOffset? gaveOutAt)
        {
            Id = id;
            MerchPack = merchPack;
            Employee = employee;
            Status = status;
            CreatedAt = createdAt;
            GaveOutAt = gaveOutAt;
        }

        public MerchPack MerchPack { get; }
        
        public Employee Employee { get; }
        
        public MerchandiseRequestStatus Status { get; private set; }
        
        public DateTimeOffset CreatedAt { get; }
        
        public DateTimeOffset? GaveOutAt { get; private set; }

        private MerchandiseRequest(MerchPack merchPack, Employee employee, DateTimeOffset createdAt)
        {
            MerchPack = merchPack;
            Employee = employee;
            CreatedAt = createdAt;
            Status = MerchandiseRequestStatus.New;
        }
        
        public static MerchandiseRequest Create(
            MerchPack merchPack,
            Employee employee,
            DateTimeOffset createdAt,
            IReadOnlyCollection<MerchandiseRequest> existingRequests)
        {
            var newRequest = new MerchandiseRequest(merchPack, employee, createdAt);
            if (newRequest.CheckAbilityToGiveOut(existingRequests, createdAt))
            {
                return newRequest;
            }            
            
            throw new DomainException("Unable to create new merchandise request");
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
                throw new DomainException($"Unable to decline merchandise for request in status: {Status}");
            }
            
            Status = MerchandiseRequestStatus.Declined;
        }

        private bool CheckAbilityToGiveOut(IReadOnlyCollection<MerchandiseRequest> existingRequests, DateTimeOffset gaveOutAt)
        {
            var existingRequestsWithSameMerch =
                existingRequests.Where(r => Equals(r.MerchPack.MerchPackType, MerchPack.MerchPackType)).ToList();

            // Если есть незавершенные заявки на такой же мерч, не разрешаем создать новую
            if (existingRequestsWithSameMerch.Any(existingRequest =>
                Equals(existingRequest.Status, MerchandiseRequestStatus.New) ||
                Equals(existingRequest.Status, MerchandiseRequestStatus.Processing)))
            {
                return false;
            }

            // Если есть завершенные заявки на такой же мерч, сделанные менее года назад, не разрешаем создать новую
            var oneYearTimeSpan = new TimeSpan(365, 0, 0, 0);
            var finishedRequestsInLessThenAYear = existingRequestsWithSameMerch
                .Where(r => 
                    Equals(r.Status, MerchandiseRequestStatus.Done) && (gaveOutAt - r.GaveOutAt) < oneYearTimeSpan);

            if (finishedRequestsInLessThenAYear.Any())
            {
                return false;
            }

            return true;
        }
    }
}