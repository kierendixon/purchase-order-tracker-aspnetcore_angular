using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchaseOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Create.CommandResult>> Create([FromBody]Create.Command command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Create.QueryResult>> Create()
        {
            var query = new Create.Query();
            return await _mediator.Send(query);
        }

        [HttpGet("{PurchaseOrderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Edit.QueryResult>> Edit([FromRoute] Edit.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("{PurchaseOrderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Edit.QueryResult>> Edit([FromBody]Edit.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{PurchaseOrderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromRoute] Delete.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("{PurchaseOrderId}/line-items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditLineItems.Result>> EditLineItems([FromRoute] EditLineItems.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPut("{PurchaseOrderId}/line-items")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateLineItem.Result>> CreateLineItem([FromBody]CreateLineItem.Command command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPost("{PurchaseOrderId}/line-items/{LineItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditLineItem.Result>> EditLineItem([FromBody]EditLineItem.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{PurchaseOrderId}/line-items/{LineItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeleteLineItem.Result>> DeleteLineItem([FromRoute] DeleteLineItem.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("{purchaseOrderId}/status")]
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
