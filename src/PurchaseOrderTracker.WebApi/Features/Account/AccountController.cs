using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.WebApi.Features.Account
{
    public class AccountController : BaseController
    {
        public AccountController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody]Login.Command command)
        {
            var result = await _mediator.Send(command);

            if (result.Succeeded)
            {
                return Ok(result.JwtToken);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new Logout.Command());

            return Ok();
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh([FromBody]Refresh.Command command)
        {
            var result = await _mediator.Send(command);

            if (result.Succeeded)
            {
                return Ok(result.Token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
