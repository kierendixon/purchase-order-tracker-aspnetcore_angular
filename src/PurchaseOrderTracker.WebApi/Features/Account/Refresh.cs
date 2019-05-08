using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PurchaseOrderTracker.Persistence.Identity;
using PurchaseOrderTracker.WebApi.Identity;

namespace PurchaseOrderTracker.WebApi.Features.Account
{
    public class Refresh
    {
        public class Command : IRequest<Result>
        {
            [Required]
            public string RefreshToken { get; set; }
        }

        public class Result
        {
            public Result(bool succeeded, JsonWebToken token = null)
            {
                Succeeded = succeeded;
                Token = token;
            }

            public bool Succeeded { get; }
            public JsonWebToken Token { get; }
        }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            {
                _userManager = userManager;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var bearerToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                var principal = TryGetPrincipalFromToken(bearerToken);
                if (principal != null)
                {
                    var username = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var user = await _userManager.FindByNameAsync(username);
                    if (user != null && TokenMatchesAndNotExpired(user, request.RefreshToken))
                    {
                        // TODO: Use IdentityServer4
                        var jwtToken = JwtFactory.Create(user);
                        await SaveRefreshToken(user, jwtToken.RefreshToken, DateTime.Now.AddDays(1));

                        return new Result(true, jwtToken);
                    }
                }

                return new Result(false);
            }

            private static bool TokenMatchesAndNotExpired(ApplicationUser user, string token)
            {
                return user.RefreshToken == token && user.RefreshTokenExpiresAt > DateTime.Now;
            }

            private ClaimsPrincipal TryGetPrincipalFromToken(string token)
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    // TODO ValidateIssuerSigningKey = true,
                    ValidIssuers = new[] { JwtConfig.Issuer },
                    ValidAudiences = new[] { JwtConfig.Audience },
                    IssuerSigningKey = JwtConfig.SigningKey,
                    ValidateLifetime = false
                };

                try
                {
                    var principal = new JwtSecurityTokenHandler()
                                .ValidateToken(token, tokenValidationParameters, out var securityToken);

                    if (!(securityToken is JwtSecurityToken jwtSecurityToken)
                        || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                    return principal;
                }
                catch (SecurityTokenException)
                {
                    return null;
                }
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
