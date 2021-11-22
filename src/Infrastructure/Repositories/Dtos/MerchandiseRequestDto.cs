using System;

namespace Infrastructure.Repositories.Dtos
{
    public class MerchandiseRequestDto
    {
        public long Id { get; set; }
        
        public long MerchPackId { get; set; }
        
        public string EmployeeEmail { get; set; }
        
        public int EmployeeClothingSizeId { get; set; }
        
        public int RequestStatusId { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? GaveOutAt { get; set; }
    }
}