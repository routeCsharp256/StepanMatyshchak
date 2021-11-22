using Domain.BaseModels;

namespace Domain.AggregationModels.MerchandiseRequest
{
    public class MerchandiseRequestStatus : Enumeration
    {
        public static MerchandiseRequestStatus New = new(1, "new");
        
        public static MerchandiseRequestStatus Processing = new(2, "processing");
        
        public static MerchandiseRequestStatus Done = new(2, "done");

        public static MerchandiseRequestStatus Declined = new(2, "declined");
        
        public static MerchandiseRequestStatus Parse(string status) => status?.ToUpper() switch
        {
            "new" => New,
            "processing" => Processing,
            "done" => Done,
            "declined" => Declined,
            _ => throw new DomainException("Invalid request status")
        };
        
        private MerchandiseRequestStatus(int id, string name) : base(id, name)
        {
        }
    }
}