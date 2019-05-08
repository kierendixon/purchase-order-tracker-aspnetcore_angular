using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PurchaseOrderTracker.Persistence.Identity;
using PurchaseOrderTracker.WebApi.Identity;

namespace PurchaseOrderTracker.WebApi.StartupExtensions.ServiceCollectionExtensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddCustomIdentity(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<Persistence.IdentityDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.SaveToken = true;
                    opt.RequireHttpsMetadata = false;
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = JwtConfig.Issuer,
                        ValidAudience = JwtConfig.Audience,
                        IssuerSigningKey = JwtConfig.SigningKey
                    };
                });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddDbContext<Persistence.IdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityDatabase")));

            return services;
        }
    }
}
