using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Application.Features.Supplier.Commands;
using PurchaseOrderTracker.Application.Features.Supplier.Queries;
using PurchaseOrderTracker.Application.PagedList;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.WebApi.Features.Supplier.Models;
using X.PagedList;
using static PurchaseOrderTracker.WebApi.Features.Supplier.Models.InquiryQueryResultDto;

namespace PurchaseOrderTracker.WebApi.Features.Supplier;

public class SupplierController : BaseController
{
    public SupplierController(IMediator mediator, IMapper mapper)
        : base(mediator, mapper)
    {
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCommand.Result>> Create(CreateCommandDto dto)
    {
        var command = new CreateCommand(new SupplierName(dto.Name));
        var result = await Mediator.Send(command);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet("{supplierId?}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EditQuery.Result>> Get(int? supplierId)
    {
        return await Mediator.Send(new EditQuery(supplierId));
    }

    [HttpPost("{supplierId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EditQuery.Result>> Update(int supplierId, EditCommandDto dto)
    {
        var command = new EditCommand(supplierId, new SupplierName(dto.Name));
        return await Mediator.Send(command);
    }

    [HttpDelete("{supplierId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(int supplierId)
    {
        await Mediator.Send(new DeleteCommand(supplierId));
        return Ok();
    }

    [HttpPut("{supplierId}/products")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateProductCommand.Result>> CreateProduct(
        int supplierId,
        CreateProductCommandDto dto)
    {
        var command = new CreateProductCommand(supplierId, new ProductCode(dto.ProdCode),
            new ProductName(dto.Name), dto.CategoryId.Value, dto.Price, dto.CategoryOptions);
        var result = await Mediator.Send(command);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet("{supplierId}/products")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EditProductsQuery.Result>> GetProducts(
        int? pageNumber,
        int? pageSize,
        [StringLength(20, MinimumLength = 3)] string productCodeFilter,
        int supplierId)
    {
        var query = new EditProductsQuery(
            pageSize,
            pageNumber,
            supplierId,
            productCodeFilter == null ? null : new ProductCode(productCodeFilter));
        return await Mediator.Send(query);
    }

    [HttpPost("{supplierId}/products/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EditProductCommand.Result>> EditProduct(
        int supplierId,
        int productId,
        EditProductCommandDto dto)
    {
        var command = new EditProductCommand(supplierId, productId, new ProductCode(dto.ProdCode),
            new ProductName(dto.Name), dto.CategoryId, dto.Price.Value);
        return await Mediator.Send(command);
    }

    [HttpDelete("{supplierId}/products/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteProductCommand.Result>> DeleteProduct(
        int supplierId,
        int productId)
    {
        return await Mediator.Send(new DeleteProductCommand(supplierId, productId));
    }

    [HttpGet("{supplierId}/product-categories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EditProductCategoriesQuery.Result>> EditProductCategories(
        int? pageNumber,
        int? pageSize,
        int supplierId)
    {
        var query = new EditProductCategoriesQuery(pageSize, pageNumber, supplierId);
        var result = await Mediator.Send(query);

        return result;
    }

    [HttpPut("{supplierId}/product-categories")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProductCategory(
        int supplierId,
        CreateProductCategoryCommandDto dto)
    {
        var command = new CreateProductCategoryCommand(supplierId, new ProductCategoryName(dto.Name));
        await Mediator.Send(command);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPost("{supplierId}/product-categories/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditProductCategory(
        int supplierId,
        int categoryId,
        EditProductCategoryCommandDto dto)
    {
        var command = new EditProductCategoryCommand(supplierId, categoryId, new ProductCategoryName(dto.Name));
        await Mediator.Send(command);

        return Ok();
    }

    [HttpDelete("{supplierId}/product-categories/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProductCategory(
        int supplierId,
        int categoryId)
    {
        await Mediator.Send(new DeleteProductCategoryCommand(supplierId, categoryId));
        return Ok();
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InquiryQueryResultDto>> Inquiry(
        int? pageSize,
        int? pageNumber,
        [Required] InquiryQuery.QueryType? queryType)
    {
        var result = await Mediator.Send(new InquiryQuery(pageSize, pageNumber, queryType.Value));
        var pagedListDto = new StaticPagedList<SupplierDto>(
            Mapper.Map<List<SupplierDto>>(result.PagedList), result.PagedList);

        return new InquiryQueryResultDto(pagedListDto.ToMinimal());
    }
}
