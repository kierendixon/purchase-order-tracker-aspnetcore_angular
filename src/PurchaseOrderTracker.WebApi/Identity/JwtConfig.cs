using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PurchaseOrderTracker.WebApi.Identity
{
    public static class JwtConfig
    {
        public const int TokenLifetimeMinutes = 30;
        public const string Audience = "po-tracker-web-server";
        private const string InsecureKey = "insecure-key-128-bits";

        public static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(InsecureKey));
        public static readonly string Issuer = typeof(JwtFactory).Assembly.GetName().Name;
    }
}
