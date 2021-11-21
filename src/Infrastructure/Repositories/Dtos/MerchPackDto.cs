using System.Collections.Generic;

namespace Infrastructure.Repositories.Dtos
{
    public class MerchPackDto
    {
        public long Id { get; set; }
        
        public int MerchPackTypeId { get; set; }
        
        public int ClothingSizeId { get; set; }

        public List<long> SkuCollection { get; set; }
    }
}