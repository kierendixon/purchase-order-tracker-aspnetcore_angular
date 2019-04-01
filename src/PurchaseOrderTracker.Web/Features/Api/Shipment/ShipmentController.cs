using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.Web.Features.Api.Shipment
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShipmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Create.Result>> Create([FromBody] Create.Command command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet("{ShipmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Edit.QueryResult>> Get(Edit.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("{ShipmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Edit.QueryResult>> Update([FromBody] Edit.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{ShipmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(Delete.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("{ShipmentId}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateStatus([FromBody]EditStatus.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Inquiry.Result>> Inquiry([FromQuery] Inquiry.Query query)
        {
            return await _mediator.Send(query);
        }
    }
}
