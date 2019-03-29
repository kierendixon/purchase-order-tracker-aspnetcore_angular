using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.Web.Features.Api.Account
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMediator _mediator;

        public AccountController(SignInManager<IdentityUser> signInManager, IMediator mediator)
        {
            _signInManager = signInManager;
            _mediator = mediator;
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
                return Ok();
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
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public ActionResult<IsAuthenticatedQueryResult> IsAuthenticated()
        {
            return new IsAuthenticatedQueryResult(User.Identity.IsAuthenticated);
        }
    }
}
