using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.Web.Features.Api.Shipment
{
    [Route("api/[controller]")]
    public class ShipmentController : Controller
    {
        private readonly IMediator _mediator;

        public ShipmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        public async Task<ActionResult> Create([FromBody] Create.Command command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                return new ObjectResult(result);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Edit.Query query)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(query);
                return new ObjectResult(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Update([FromBody] Edit.Command command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Delete.Command command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost("{id}/status")]
        public async Task<ActionResult> UpdateStatus([FromBody]EditStatus.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Inquiry(Inquiry.Query query)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(query);
                return new ObjectResult(result);
            }
            return BadRequest(ModelState);
        }
    }
}