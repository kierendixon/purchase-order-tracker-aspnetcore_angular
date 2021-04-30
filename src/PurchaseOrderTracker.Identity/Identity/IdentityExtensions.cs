using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;
using PurchaseOrderTracker.Identity.Features.Account;
using PurchaseOrderTracker.Identity.Persistence;

namespace PurchaseOrderTracker.Identity
{
    // TODO move Identity into a separate web application and use IdentityServer
    // https://identityserver4.readthedocs.io/en/latest/
    public static class IdentityExtensions
    {
        public const string Scheme = CookieAuthenticationDefaults.AuthenticationScheme;

        private static readonly Action<CookieAuthenticationOptions> _configureCookies = opt =>
        {
            opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            opt.Cookie.Name = "pot.session";
            opt.Cookie.IsEssential = true;
            opt.Cookie.HttpOnly = true;

            opt.Events.OnRedirectToLogin = context =>
            {
                // TODO handle html requests
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

            opt.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };

            opt.Events.OnRedirectToLogout = context =>
            {
                // override default event which calls
                // context.Response.Redirect(context.RedirectUri)

                return Task.CompletedTask;
            };

            opt.Events.OnRedirectToReturnUrl = context =>
            {
                // override default event which calls
                // context.Response.Redirect(context.RedirectUri)
                return Task.CompletedTask;
            };

            // TODO 
            // default ISecurityStampValidator implementation relies on SignInManager
            // opt.Events.OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
        };

        public static IServiceCollection AddCustomIdentity(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(Scheme)
                .AddCookie(_configureCookies);

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityDatabase")));

            // Add UserManager and its dependencies
            //services.AddOptions().AddLogging();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
            services.AddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
            services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
            services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.AddScoped<IdentityErrorDescriber>();
            // TODO
            //services.AddScoped<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();
            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AdditionalUserClaimsPrincipalFactory>();
            //services.AddScoped<IUserConfirmation<ApplicationUser>, DefaultUserConfirmation<ApplicationUser>>();
            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();

            services.Configure<IdentityOptions>(opt =>
            {
                // relax password requirements for testing purposes
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequiredUniqueChars = 1;

                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // this is the default in ASP.Net Identity
            });

            // TODO: still needed?
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator",
                    new AuthorizationPolicyBuilder()
                        .RequireClaim(ClaimTypes.Role, "admin")
                        .Build()
                );

                options.AddPolicy("ApiScope", policy =>
                {
                    // TODO do we need both require auth and require claim?
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });
            });

            services.AddDataProtection()
                .SetApplicationName("PurchaseOrderTrackerApp");

            return services;
        }
    }
}
