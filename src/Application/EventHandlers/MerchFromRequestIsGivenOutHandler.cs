using System.Threading;
using System.Threading.Tasks;
using Application.Integration;
using Domain.AggregationModels.MerchandiseRequest.DomainEvents;
using MediatR;

namespace Application.EventHandlers
{
    public class MerchFromRequestIsGivenOutHandler : INotificationHandler<MerchFromRequestIsGivenOut>
    {
        private readonly IEmailService _emailService;

        public MerchFromRequestIsGivenOutHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(MerchFromRequestIsGivenOut notification, CancellationToken cancellationToken)
        {
            await _emailService.SendEmail(notification.Employee.Email,
                new {Header = "Выдача мерча", Body = "Получите мерч у HR"});
        }
    }
}