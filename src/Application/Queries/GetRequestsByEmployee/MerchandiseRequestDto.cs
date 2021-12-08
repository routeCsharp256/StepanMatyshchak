using System;

namespace Application.Queries.GetRequestsByEmployee
{
    public class MerchandiseRequestDto
    {
        public string Type { get; init; }
        
        public string Status { get; init; }

        public DateTimeOffset CreatedAt { get; init; }
        
        public DateTimeOffset? GaveOutAt { get; init; }
    }
}