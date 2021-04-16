using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Application.Features.User.Queries;
using PurchaseOrderTracker.Application.Features.User.Commands;

namespace PurchaseOrderTracker.WebApi.Features.User
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }

        // TODO
        [Authorize("Administrators")]
        [HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUsersQuery.Result>> Get(int pageSize)
        {
            return await _mediator.Send(new GetUsersQuery(pageSize, null));
        }

        // TODO
        [Authorize("Administrators")]
        [HttpPut]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateCommand.Result>> Create([FromBody] CreateCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
