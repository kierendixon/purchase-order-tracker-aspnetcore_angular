using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Web.Infrastructure;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    [Route("api/[controller]")]
    public class SupplierController : Controller
    {
        private readonly IMediator _mediator;

        public SupplierController(IMediator mediator)
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

        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(Edit.Query query)
        {
            var result = await _mediator.Send(query);
            return new ObjectResult(result);
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

        [HttpPut("{id}/products")]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProduct.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProducts(EditProducts.Query query)
        {
            var result = await _mediator.Send(query);
            return new ObjectResult(result);
        }

        [HttpPost("{supplierId}/products/{productId}")]
        public async Task<IActionResult> EditProduct([FromBody] EditProduct.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{supplierId}/products/{productId}")]
        public async Task<IActionResult> DeleteProduct(DeleteProduct.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}/product-categories")]
        public async Task<IActionResult> EditProductCategories(EditProductCategories.Query query)
        {
            var result = await _mediator.Send(query);
            return new ObjectResult(result);
        }

        [HttpPut("{id}/product-categories")]
        public async Task<IActionResult> CreateProductCategory([FromBody]CreateProductCategory.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("{supplierId}/product-categories/{categoryId}")]
        public async Task<IActionResult> EditProductCategory([FromBody]EditProductCategory.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{supplierId}/product-categories/{categoryId}")]
        public async Task<IActionResult> DeleteProductCategory(DeleteProductCategory.Command command)
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