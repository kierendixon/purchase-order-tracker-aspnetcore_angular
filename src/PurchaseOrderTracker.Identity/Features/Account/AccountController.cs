using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderTracker.Identity;
using PurchaseOrderTracker.Identity.Features;
using PurchaseOrderTracker.Identity.Features.Account;
using PurchaseOrderTracker.Identity.Features.Account.Models;

namespace PurchaseOrderTracker.WebApi.Features.Account
{
    [Route("identity/[controller]/[action]")] // TODO shouldnt need identity prefix. Set this in basecontroller?
    public class AccountController : BaseController
    {
        public AccountController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }

        // TODO configure rate limiting in envoy for this route
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginCommandDto dto)
        {
            var result = await _mediator.Send(new LoginCommand(dto.UserName, dto.Password));

            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(IdentityServiceCollectionExtensions.Scheme);
            }

            return Ok();
        }
    }
}
