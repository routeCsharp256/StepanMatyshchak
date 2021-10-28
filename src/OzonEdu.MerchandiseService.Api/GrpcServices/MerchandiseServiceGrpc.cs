using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OzonEdu.MerchandiseService.Api.HttpModels.OrderMerch;
using OzonEdu.MerchandiseService.Api.Services.Interfaces;
using OzonEdu.MerchandiseService.Grpc;

namespace OzonEdu.MerchandiseService.Api.GrpcServices
{
    public class MerchandiseGrpcService : MerchandiseServiceGrpc.MerchandiseServiceGrpcBase
    {
        private readonly IMerchService _merchService;

        public MerchandiseGrpcService(IMerchService merchService)
        {
            _merchService = merchService;
        }

        public override async Task<OrderMerchGrpcResponse> OrderMerch(OrderMerchGrpcRequest request, ServerCallContext context)
        { 
            // TODO mapping
            var httpRequest = new OrderMerchRequest();
            var httpResponse = await _merchService.OrderMerch(httpRequest, context.CancellationToken);
            return new OrderMerchGrpcResponse();
        }

        public override async Task<GetMerchInfoForEmployeeGrpcResponse> GetMerchInfoForEmployee(Int64Value request, ServerCallContext context)
        {
            // TODO mapping
            var employeeId = request.Value;
            var httpResponse = await _merchService.GetMerchInfoForEmployee(employeeId, context.CancellationToken);
            return new GetMerchInfoForEmployeeGrpcResponse();
        }
    }
}