using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupplierController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPut]
        public async Task<ActionResult> Create([FromBody] Create.Command command)
        {
            var result = await _mediator.Send(command);
            return new ObjectResult(result);
        }

        [HttpGet("{supplierId?}")]
        public async Task<IActionResult> Get(Edit.Query query)
        {
            var result = await _mediator.Send(query);
            return new ObjectResult(result);
        }

        [HttpPost("{supplierId}")]
        public async Task<ActionResult> Update([FromBody] Edit.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{supplierId}")]
        public async Task<ActionResult> Delete(Delete.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{supplierId}/products")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProduct.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{supplierId}/products")]
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
        
        [HttpGet("{supplierId}/product-categories")]
        public async Task<IActionResult> EditProductCategories(EditProductCategories.Query query)
        {
            var result = await _mediator.Send(query);
            return new ObjectResult(result);
        }

        [HttpPut("{supplierId}/product-categories")]
        public async Task<IActionResult> CreateProductCategory([FromBody] CreateProductCategory.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("{supplierId}/product-categories/{categoryId}")]
        public async Task<IActionResult> EditProductCategory([FromBody] EditProductCategory.Command command)
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
            var result = await _mediator.Send(query);
            return new ObjectResult(result);
        }
    }
}