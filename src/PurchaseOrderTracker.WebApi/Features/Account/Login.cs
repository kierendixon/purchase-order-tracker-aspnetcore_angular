using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PurchaseOrderTracker.Persistence.Identity;
using PurchaseOrderTracker.WebApi.Identity;

namespace PurchaseOrderTracker.WebApi.Features.Account
{
    public class Login
    {
        public class Command : IRequest<CommandResult>
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class CommandResult
        {
            public CommandResult(bool succeeded, JsonWebToken jwtToken = null)
            {
                Succeeded = succeeded;
                JwtToken = jwtToken;
            }

            public bool Succeeded { get; }
            public JsonWebToken JwtToken { get; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;

            public CommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    return new CommandResult(false);
                }

                var passwordIsValid = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!passwordIsValid)
                {
                    return new CommandResult(false);
                }

                if (request.UserName == "basic")
                {
                    var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
                }

                // TODO to create cookies use _signInManager

                // TODO: Use IdentityServer4
                var jwtToken = JwtFactory.Create(user);
                await SaveRefreshToken(user, jwtToken.RefreshToken, DateTime.Now.AddDays(1));

                return new CommandResult(true, jwtToken);
            }

            private async Task SaveRefreshToken(ApplicationUser user, string token, DateTime expiresAt)
            {
                user.RefreshToken = token;
                user.RefreshTokenExpiresAt = expiresAt;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
