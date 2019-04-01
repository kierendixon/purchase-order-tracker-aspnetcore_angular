using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.Web.Features.Api.Reporting
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ShipmentsSummary.Result>> ShipmentsSummary()
        {
            // Build the query here instead of as a parameter to avoid Swagger generating a parameter with:
            // "type":"object"
            var query = new ShipmentsSummary.Query();
            return await _mediator.Send(query);
        }
    }
}
