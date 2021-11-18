using System.Collections.Generic;
using Domain.BaseModels;

namespace Domain.AggregationModels.MerchandiseRequest
{
    public class Employee : ValueObject
    {
        public Employee(Email email, ClothingSize clothingSize)
        {
            Email = email;
            ClothingSize = clothingSize;
        }
        
        public Email Email { get; }
        
        public ClothingSize ClothingSize { get; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Email;
            yield return ClothingSize;
        }
    }
}