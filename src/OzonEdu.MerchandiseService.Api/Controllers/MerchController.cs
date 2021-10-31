using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.Api.HttpModels.GetMerchInfoForEmployee;
using OzonEdu.MerchandiseService.Api.HttpModels.OrderMerch;
using OzonEdu.MerchandiseService.Api.Services.Interfaces;

namespace OzonEdu.MerchandiseService.Api.Controllers
{
    [ApiController]
    [Route("v1/api")]
    [Produces("application/json")]
    public class MerchController : ControllerBase
    {
        private readonly IMerchService _merchService;

        public MerchController(IMerchService merchService)
        {
            _merchService = merchService;
        }
        
        [HttpPost("order-merch")]
        public async Task<IActionResult> OrderMerch(OrderMerchRequest orderMerchRequest,
            CancellationToken cancellationToken)
        {
            var result = await _merchService.OrderMerch(orderMerchRequest, cancellationToken);
            return Ok(result);
        }
        
        [HttpGet("get-merch-info-for-employee/{employeeId:long}")]
        public async Task<ActionResult<GetMerchInfoForEmployeeResponse>> GetMerchInfoForEmployee(
            [FromRoute] long employeeId,
            CancellationToken cancellationToken)
        {
            var result = await _merchService.GetMerchInfoForEmployee(employeeId, cancellationToken);
            return Ok(result);
        }
    }
}