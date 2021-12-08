using System;

namespace Domain.BaseModels
{
    public class DomainException : Exception
    {
        public DomainException(string description) : base(description)
        {
        }
        
        public DomainException(string description, Exception innerException) : base(description, innerException)
        {
        }
    }
}