using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.Web.Features.Api.Reporting
{
    [Route("api/[controller]")]
    public class ReportingController : Controller
    {
        private readonly IMediator _mediator;

        public ReportingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ShipmentsSummary(ShipmentsSummary.Query query)
        {
            var result = await _mediator.Send(query);
            return new ObjectResult(result);
        }
    }
}