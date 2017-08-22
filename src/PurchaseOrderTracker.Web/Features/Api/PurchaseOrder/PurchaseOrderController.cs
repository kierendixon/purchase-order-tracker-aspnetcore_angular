using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    [Route("api/[controller]")]
    public class PurchaseOrderController : Controller
    {
        private readonly IMediator _mediator;

        public PurchaseOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        public async Task<ActionResult> Create([FromBody]Create.Command command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                return new ObjectResult(result);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<ActionResult> Create([FromBody]Create.Query query)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(query);
                return new ObjectResult(result);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Edit(Edit.Query query)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Edit([FromBody]Edit.Command command)
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

        [HttpGet("{id}/line-items")]
        public async Task<IActionResult> EditLineItems(EditLineItems.Query query)
        {
            var result = await _mediator.Send(query);
            return new ObjectResult(result);
        }

        [HttpPut("{id}/line-items")]
        public async Task<IActionResult> CreateLineItem([FromBody]CreateLineItem.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/line-items/{lineItemId}")]
        public async Task<IActionResult> EditLineItem([FromBody]EditLineItem.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}/line-items/{lineItemId}")]
        public async Task<IActionResult> DeleteLineItem(DeleteLineItem.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
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