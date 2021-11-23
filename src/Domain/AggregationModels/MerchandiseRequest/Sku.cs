using System.Collections.Generic;
using Domain.BaseModels;

namespace Domain.AggregationModels.MerchandiseRequest
{
    public class Sku : ValueObject
    {        
        private Sku(long sku) => Value = sku;
        
        public long Value { get; }

        public static Sku Create(long skuValue)
        {
            if (skuValue > 0)
            {
                return new Sku(skuValue);
            }

            throw new DomainException($"SKU value is invalid: {skuValue}");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}