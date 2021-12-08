using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Api.HttpModels.GetMerchInfoForEmployee;
using OzonEdu.MerchandiseService.Api.HttpModels.OrderMerch;

namespace OzonEdu.MerchandiseService.Api.Services.Interfaces
{
    public interface IMerchService
    {
        Task<OrderMerchResponse> OrderMerch(OrderMerchRequest orderMerchRequest, CancellationToken cancellationToken);
        
        Task<GetMerchInfoForEmployeeResponse> GetMerchInfoForEmployee(long employeeId,
            CancellationToken cancellationToken);
    }
}