using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Application.Features.User.Queries;

namespace PurchaseOrderTracker.WebApi.Features.User
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }

        // TODO
        [AllowAnonymous]
        [HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUsersQuery.Result>> Get()
        {
            return await _mediator.Send(new GetUsersQuery(null,null));
        }
    }
}
