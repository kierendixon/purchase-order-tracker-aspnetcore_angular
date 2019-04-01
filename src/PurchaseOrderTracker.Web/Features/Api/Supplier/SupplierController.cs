using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Create.CommandResult>> Create([FromBody] Create.Command command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet("{SupplierId?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Edit.QueryResult>> Get([FromRoute] Edit.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("{SupplierId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Edit.QueryResult>> Update([FromBody] Edit.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{SupplierId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(Delete.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{SupplierId}/products")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateProduct.CommandResult>> CreateProduct([FromBody] CreateProduct.Command command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet("{SupplierId}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditProducts.Result>> GetProducts(EditProducts.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("{SupplierId}/products/{ProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditProduct.Result>> EditProduct([FromBody] EditProduct.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{SupplierId}/products/{ProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeleteProduct.Result>> DeleteProduct([FromRoute] DeleteProduct.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("{SupplierId}/product-categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditProductCategories.Result>> EditProductCategories(EditProductCategories.Query query)
        {
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpPut("{SupplierId}/product-categories")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProductCategory([FromBody] CreateProductCategory.Command command)
        {
            await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("{SupplierId}/product-categories/{CategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditProductCategory([FromBody] EditProductCategory.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{SupplierId}/product-categories/{CategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProductCategory([FromRoute] DeleteProductCategory.Command command)
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
