using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;
using static PurchaseOrderTracker.Identity.Features.Account.LoginCommand;

namespace PurchaseOrderTracker.Identity.Features.Account
{
    public class LoginCommand : IRequest<Result>
    {
        public LoginCommand(string userName, string password)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public string UserName { get; }
        public string Password { get; }

        public class Result
        {
            public Result(bool succeeded)
            {
                Succeeded = succeeded;
            }

            public bool Succeeded { get; }
        }

        public class Handler : IRequestHandler<LoginCommand, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            {
                _userManager = userManager;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user != null && !IsUserAccountLocked(user))
                {
                    if (await _userManager.CheckPasswordAsync(user, request.Password))
                    {
                        var resetAccessFailedTask = _userManager.ResetAccessFailedCountAsync(user);

                        await _httpContextAccessor.HttpContext.SignInAsync(
                            IdentityServiceCollectionExtensions.Scheme,
                            new ClaimsPrincipal(GenerateClaims(user)));

                        await resetAccessFailedTask;

                        return new Result(true);
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);
                    }
                }

                return new Result(false);
            }

            private static bool IsUserAccountLocked(ApplicationUser user)
            {
                // TODO user.LockoutEnd.DateTime ??
                return user.LockoutEnabled == true
                    && user.LockoutEnd?.LocalDateTime > DateTime.Now;
            }

            private ClaimsIdentity GenerateClaims(ApplicationUser user)
            {
                //var userId = await _userManager.GetUserIdAsync(user);
                //var userName = await _userManager.GetUserNameAsync(user);

                var id = new ClaimsIdentity(IdentityServiceCollectionExtensions.Scheme);
                id.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                id.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
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
