using System.Collections.Generic;
using Domain.AggregationModels.MerchandiseRequest;
using Domain.BaseModels;

namespace Domain.AggregationModels.MerchPack
{
    public class MerchPack : Entity
    {
        public MerchPack(long id,
            MerchPackType merchPackType,
            ClothingSize clothingSize,
            IReadOnlyCollection<Sku> skuCollection)
        {
            Id = id;
            MerchPackType = merchPackType;
            ClothingSize = clothingSize;
            SkuCollection = skuCollection;
        }

        public MerchPackType MerchPackType { get; }
        
        public ClothingSize ClothingSize { get; }
        
        public IReadOnlyCollection<Sku> SkuCollection { get; } 
    }
}