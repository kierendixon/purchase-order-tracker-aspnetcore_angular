using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Application.Features.Reporting.Queries;

namespace PurchaseOrderTracker.WebApi.Features.Reporting
{
    public class ReportingController : BaseController
    {
        public ReportingController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ShipmentsSummaryQuery.Result>> ShipmentsSummary()
        {
            return await _mediator.Send(new ShipmentsSummaryQuery());
        }
    }
}
