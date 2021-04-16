using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOrderTracker.Persistence.Identity;
using PurchaseOrderTracker.WebApi.Identity;

namespace PurchaseOrderTracker.WebApi.StartupExtensions.ServiceCollectionExtensions
{
    public static class IdentityExtensions
    {
        public const string Scheme = CookieAuthenticationDefaults.AuthenticationScheme;

        public static IServiceCollection AddCustomIdentity(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(Scheme)
                .AddCookie(opt =>
                {
                    opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    opt.Cookie.Name = "pot.session";

                    opt.LoginPath = new PathString("/account"); // challenge path
                    // TODO frontend needs to handle this when performing api calls
                    // or for api calls use a different scheme?
                    //opt.AccessDeniedPath = ""; // forbid path
                    //opt.LogoutPath = ""; // where to redirect after logout
                    
                    //opt.Events = new CookieAuthenticationEvents
                    //{
                    //    // TODO 
                    //    // default ISecurityStampValidator implementation relies on SignInManager
                    //    OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                    //};
                });

            services.AddDbContext<Persistence.IdentityDbContext>(options =>
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

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;
            });

            return services;
        }
    }
}
