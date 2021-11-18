using System.Collections.Generic;
using Domain.BaseModels;

namespace Domain.AggregationModels.MerchPack
{
    public class MerchPack : Entity
    {        
        public MerchPackType MerchPackType { get; }
        
        public IReadOnlyCollection<Sku> MerchPackItemSkus { get; } 
    }
}