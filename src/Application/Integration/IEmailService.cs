using System.Threading.Tasks;
using Domain.AggregationModels.MerchandiseRequest;

namespace Application.Integration
{
    public interface IEmailService
    {
        Task SendEmail(Email employeeEmail, object o);
    }
}