using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Application.Features.User.Commands;
using PurchaseOrderTracker.Application.Features.User.Queries;

namespace PurchaseOrderTracker.WebApi.Features.User
{
    [Authorize(RoleAdministrator)]
    public class UserController : BaseController
    {
        // todo
        public const string RoleAdministrator = "administrator";

        public UserController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<GetUsersQuery.Result>> Get(
            [Required][Range(1,20)] int? pageSize,
            string filter,
            int page = 1)
        {
            return await _mediator.Send(new GetUsersQuery(pageSize, filter, page));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateCommand.Result>> Create([FromBody] CreateCommand command)
        {
            // Todo map to RPC error response format?
            // todo returmn http201 for created?s
            return await _mediator.Send(command);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateCommand.Result>> Update([FromBody] UpdateCommand command)
        {
            // todo finalise
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<DeleteCommand.Result> Delete(string id)
        {
            // todo return 404 if not found
            return await _mediator.Send(new DeleteCommand(id));
        }
    }
}
