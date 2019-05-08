using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace PurchaseOrderTracker.WebApi.Identity
{
    public static class JwtFactory
    {
        public static JsonWebToken Create(IdentityUser user)
        {
            var signingCredentials = new SigningCredentials(JwtConfig.SigningKey, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.Now.AddMinutes(JwtConfig.TokenLifetimeMinutes);
            var subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            });

            var serializedJwtToken = new JwtSecurityTokenHandler().CreateEncodedJwt(
                JwtConfig.Issuer,
                JwtConfig.Audience,
                subject,
                null,
                expiresAt,
                DateTime.Now,
                signingCredentials);

            // guid generation is predictable and therefore not secure
            var refreshToken = Guid.NewGuid().ToString();
            var expiresInSeconds = JwtConfig.TokenLifetimeMinutes * 60;

            return new JsonWebToken(serializedJwtToken, expiresInSeconds, refreshToken);
        }
    }
}
