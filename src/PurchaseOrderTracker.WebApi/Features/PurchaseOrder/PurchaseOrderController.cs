using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands;
using PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries;
using PurchaseOrderTracker.Application.PagedList;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models;
using X.PagedList;
using static PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries.InquiryQuery;
using static PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models.InquiryQueryResultDto;

namespace PurchaseOrderTracker.WebApi.Features.PurchaseOrder;

public class PurchaseOrderController : BaseController
{
    public PurchaseOrderController(IMediator mediator, IMapper mapper)
        : base(mediator, mapper)
    {
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCommand.Result>> Create(CreateCommandDto dto)
    {
        var command = new CreateCommand(new OrderNo(dto.OrderNo), dto.SupplierId.Value);
        var result = await Mediator.Send(command);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CreateQuery.Result>> Create()
    {
        return await Mediator.Send(new CreateQuery());
    }

    [HttpGet("{purchaseOrderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EditQuery.Result>> Edit(int purchaseOrderId)
    {
        return await Mediator.Send(new EditQuery(purchaseOrderId));
    }

    [HttpPost("{purchaseOrderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EditQuery.Result>> Edit(
        [Required] int? purchaseOrderId,
        EditCommandDto dto)
    {
        var command = new EditCommand(
            purchaseOrderId.Value,
            dto.SupplierId.Value,
            new OrderNo(dto.OrderNo),
            dto.ShipmentId);

        return await Mediator.Send(command);
    }

    [HttpDelete("{purchaseOrderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(int purchaseOrderId)
    {
        await Mediator.Send(new DeleteCommand(purchaseOrderId));
        return Ok();
    }

    [HttpGet("{purchaseOrderId}/line-items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EditLineItemsQueryResultDto>> EditLineItems(int purchaseOrderId)
    {
        var result = await Mediator.Send(new EditLineItemsQuery(purchaseOrderId));
        return Mapper.Map<EditLineItemsQueryResultDto>(result);
    }

    [HttpPut("{purchaseOrderId}/line-items")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateLineItemCommand.Result>> CreateLineItem(
        int purchaseOrderId,
        CreateLineCommandItemDto dto)
    {
        var command = new CreateLineItemCommand(purchaseOrderId, dto.ProductId.Value,
            dto.PurchaseQty.Value, dto.PurchasePrice);
        var result = await Mediator.Send(command);

        // TODO: don't return the purchase order id. Should return line item instead
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("{purchaseOrderId}/line-items/{lineItemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EditLineItemCommand.Result>> EditLineItem(
        int purchaseOrderId,
        int lineItemId,
        EditLineItemCommandDto dto)
    {
        var command = new EditLineItemCommand(
            purchaseOrderId,
            lineItemId,
            dto.ProductId.Value,
            dto.PurchaseQty.Value,
            dto.PurchasePrice.Value);

        return await Mediator.Send(command);
    }

    [HttpDelete("{purchaseOrderId}/line-items/{lineItemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteLineItemCommand.Result>> DeleteLineItem(
        int purchaseOrderId,
        int lineItemId)
    {
        return await Mediator.Send(new DeleteLineItemCommand(purchaseOrderId, lineItemId));
    }

    [HttpPost("{purchaseOrderId}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateStatus(
        int purchaseOrderId,
        EditStatusCommandDto dto)
    {
        await Mediator.Send(new EditStatusCommand(purchaseOrderId, dto.UpdatedStatus.Value));
        return Ok();
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InquiryQueryResultDto>> Inquiry(
        int? pageSize,
        int? pageNumber,
        [Required] QueryType? queryType)
    {
        var result = await Mediator.Send(new InquiryQuery(pageSize, pageNumber, queryType.Value));
        var pagedListDto = new StaticPagedList<PurchaseOrderDto>(
            Mapper.Map<List<PurchaseOrderDto>>(result.PagedList), result.PagedList);

        return new InquiryQueryResultDto(pagedListDto.ToMinimal());
    }
}
