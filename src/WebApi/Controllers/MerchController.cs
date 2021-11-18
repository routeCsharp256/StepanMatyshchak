using System.Threading;
using System.Threading.Tasks;
using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.Api.HttpModels.GetMerchInfoForEmployee;
using OzonEdu.MerchandiseService.Api.HttpModels.OrderMerch;

namespace OzonEdu.MerchandiseService.Api.Controllers
{
    [ApiController]
    [Route("v1/api")]
    [Produces("application/json")]
    public class MerchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("order-merch")]
        public async Task<IActionResult> OrderMerch(OrderMerchRequest orderMerchRequest,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GiveOutMerchandise(), cancellationToken);
            return Ok(result);
        }
        
        [HttpGet("get-merch-info-for-employee/{employeeId:long}")]
        public async Task<ActionResult<GetMerchInfoForEmployeeResponse>> GetMerchInfoForEmployee(
            [FromRoute] long employeeId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetRequestsByEmployee(), cancellationToken);
            return Ok(result);
        }
    }
}