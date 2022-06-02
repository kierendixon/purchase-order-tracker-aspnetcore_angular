using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Application.Features.Shipment.Commands;
using PurchaseOrderTracker.Application.Features.Shipment.Queries;
using PurchaseOrderTracker.Application.PagedList;
using PurchaseOrderTracker.WebApi.Features.Shipment.Models;
using X.PagedList;
using static PurchaseOrderTracker.WebApi.Features.Shipment.Models.InquiryQueryResultDto;

namespace PurchaseOrderTracker.WebApi.Features.Shipment
{
    public class ShipmentController : BaseController
    {
        public ShipmentController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateCommand.Result>> Create(CreateCommandDto dto)
        {
            var command = Mapper.Map<CreateCommand>(dto);
            var result = await Mediator.Send(command);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet("{shipmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditQuery.Result>> Get(int shipmentId)
        {
            return await Mediator.Send(new EditQuery(shipmentId));
        }

        [HttpPost("{shipmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditQuery.Result>> Update(
            int shipmentId,
            EditCommandDto dto)
        {
            var command = new EditCommand(shipmentId, dto.TrackingId, dto.Company, dto.EstimatedArrivalDate,
                dto.Comments, dto.ShippingCost, dto.DestinationAddress);

            return await Mediator.Send(command);
        }

        [HttpDelete("{shipmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([Required] int? shipmentId)
        {
            await Mediator.Send(new DeleteCommand(shipmentId.Value));
            return Ok();
        }

        [HttpPost("{shipmentId}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateStatus( // TODO should this be ActionResult or IActionResult? Check if swagger docs changes
            int shipmentId,
            EditStatusCommandDto dto)
        {
            var command = new EditStatusCommand(shipmentId, dto.UpdatedStatus.Value);
            await Mediator.Send(command);

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
            var pagedListDto = new StaticPagedList<ShipmentDto>(
                Mapper.Map<List<ShipmentDto>>(result.PagedList), result.PagedList);

            return new InquiryQueryResultDto(pagedListDto.ToMinimal());
        }
    }
}
