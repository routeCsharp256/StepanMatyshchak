using System.Collections.Generic;
using Domain.BaseModels;

namespace Domain.AggregationModels.MerchandiseRequest
{
    public class Email : ValueObject
    {
        public string Value { get; }
        
        private Email(string name) => Value = name;

        public static Email Create(string emailString)
        {
            if (IsValidEmail(emailString))
            {
                return new Email(emailString);
            }

            throw new DomainException($"Email is invalid: {emailString}");
        }

        private static bool IsValidEmail(string emailString)
        {
            // Сделать валидацию
            return true;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}