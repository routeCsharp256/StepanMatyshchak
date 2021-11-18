using System.Collections.Generic;

namespace Application.Queries.GetRequestsByEmployee
{
    public class GetRequestsByEmployeeQueryResponse
    {
        public IReadOnlyCollection<MerchandiseRequestDto> Items { get; set; }
    }
}