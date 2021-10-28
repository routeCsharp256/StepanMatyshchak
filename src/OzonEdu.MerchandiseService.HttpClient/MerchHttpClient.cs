using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Api.HttpModels.GetMerchInfoForEmployee;
using OzonEdu.MerchandiseService.Api.HttpModels.OrderMerch;

namespace OzonEdu.MerchandiseService.HttpClient
{
    public interface IMerchHttpClient
    {
        Task<OrderMerchResponse> OrderMerch(OrderMerchRequest orderMerchRequest, CancellationToken cancellationToken);
        
        Task<GetMerchInfoForEmployeeResponse> GetMerchInfoForEmployee(long employeeId,
            CancellationToken cancellationToken);
    }

    public class MerchHttpClient : IMerchHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        public MerchHttpClient(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OrderMerchResponse> OrderMerch(OrderMerchRequest orderMerchRequest, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync("v1/api/order-merch", cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<OrderMerchResponse>(body);
        }

        public async Task<GetMerchInfoForEmployeeResponse> GetMerchInfoForEmployee(long employeeId,
            CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync($"v1/api/get-merch-info-for-employee/{employeeId}", cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<GetMerchInfoForEmployeeResponse>(body);
        }
    }
}