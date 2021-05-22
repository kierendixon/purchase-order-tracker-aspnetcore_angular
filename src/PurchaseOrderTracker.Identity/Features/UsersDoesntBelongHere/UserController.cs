using AutoMapper;
using MediatR;

namespace PurchaseOrderTracker.Identity.Features.Account
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        // TODO
        //[Authorize("ApiScope", AuthenticationSchemes = "Bearer")]
        //[HttpGet]
        //public async Task<ActionResult<GetUsersQuery.Result>> Get(int pageSize)
        //{
        //    return await _mediator.Send(new GetUsersQuery(pageSize, null));
        //}

        // TODO
        //[Authorize("ApiScope", AuthenticationSchemes = "Bearer")]
        //[HttpPut]
        ////[ProducesResponseType(StatusCodes.Status200OK)]
        ////[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<CreateCommand.Result>> Create([FromBody] CreateCommand command)
        //{
        //    return await _mediator.Send(command);
        //}
    }
}
