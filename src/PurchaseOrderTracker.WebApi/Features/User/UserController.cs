using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Application.Features.User.Commands;
using PurchaseOrderTracker.Application.Features.User.Queries;
using static PurchaseOrderTracker.WebApi.IdentityServiceCollectionExtensions;

namespace PurchaseOrderTracker.WebApi.Features.User
{
    [Authorize(RoleAdministrator)]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(GetUsersQuery.Result))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetUsersQuery.Result))]
        public async Task<ActionResult<GetUsersQuery.Result>> Get(
            [Required][Range(1, 20)] int? pageSize,
            string filter,
            int page = 1)
        {
            return await _mediator.Send(new GetUsersQuery(pageSize, filter, page));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateCommand.Result))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CreateCommand.Result))]
        public async Task<ActionResult> Create([FromBody] CreateCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Succeeded)
            {
                return new JsonResult(result)
                {
                    StatusCode = StatusCodes.Status201Created,
                };
            }

            return BadRequest(result);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateCommand.Result))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UpdateCommand.Result))]
        public async Task<ActionResult> Update([FromBody] UpdateCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteCommand.Result))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteCommand.Result))]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new DeleteCommand(id));
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
