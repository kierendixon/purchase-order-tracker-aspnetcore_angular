using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PurchaseOrderTracker.Persistence.Identity;
using PurchaseOrderTracker.WebApi.Identity;
using PurchaseOrderTracker.WebApi.StartupExtensions.ServiceCollectionExtensions;

namespace PurchaseOrderTracker.WebApi.Features.Account
{
    public class Login
    {
        public class Command : IRequest<CommandResult>
        {
            // TODO: move into ctor. Use separate DTO with data annotations
            [Required]
            public string UserName { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class CommandResult
        {
            public CommandResult(bool succeeded)
            {
                Succeeded = succeeded;
            }

            public bool Succeeded { get; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            {
                _userManager = userManager;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    return new CommandResult(false);
                }

                // TODO check if account is locked
                // and handle reset failed access count on success
                var result = await _userManager.CheckPasswordAsync(user, request.Password);

                if (result)
                {
                    var claimsIdentity = await GenerateClaims(user);
                    var userPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await _httpContextAccessor.HttpContext.SignInAsync(IdentityExtensions.Scheme, userPrincipal);

                    return new CommandResult(true);
                }
                return new CommandResult(result);
            }

            private async Task<ClaimsIdentity> GenerateClaims(ApplicationUser user)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var userName = await _userManager.GetUserNameAsync(user);

                var id = new ClaimsIdentity(IdentityExtensions.Scheme);
                id.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
                id.AddClaim(new Claim(ClaimTypes.Name, userName));
                id.AddClaim(new Claim(ClaimTypes.Role, user.IsAdmin ? "admin" : "user"));

                // TODO
                //if (_userManager.SupportsUserSecurityStamp)
                //{
                //    id.AddClaim(new Claim("AspNet.Identity.SecurityStamp",
                //        await _userManager.GetSecurityStampAsync(user)));
                //}

                return id;
            }
        }
    }
}
