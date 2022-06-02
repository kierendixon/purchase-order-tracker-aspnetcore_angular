using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using MediatR;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;
using PurchaseOrderTracker.Identity.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PurchaseOrderTracker.AspNet.Common.HealthChecks;
using System.IO;
using PurchaseOrderTracker.Identity.Identity;

namespace PurchaseOrderTracker.Identity
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMediatR();
            services.AddControllers();
            services.AddCustomIdentity(_configuration, _env);
            services.AddHttpContextAccessor();
            services.AddCustomHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomErrorHandler(env);
            app.UseCustomHsts(env);
            app.UseCustomEndpoints();
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddCustomHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<IdentityDbContext>();
        }
    }

    public static class IdentityServiceCollectionExtensions
    {
        public const string Scheme = CookieAuthenticationDefaults.AuthenticationScheme;

        private static readonly Action<CookieAuthenticationOptions> ConfigureCookies = opt =>
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
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddAuthentication(Scheme)
                .AddCookie(ConfigureCookies);

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityDatabase")));

            // Add UserManager and its dependencies
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

            // TODO not needed in this application because it doesnt create users
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

            var dataProtection = services.AddDataProtection()
                .SetApplicationName("PurchaseOrderTrackerApp");

            if (!environment.IsDevelopment())
            {
                // not secure - keys will be saved to file system unencrypted
                dataProtection.PersistKeysToFileSystem(new DirectoryInfo(configuration["DataProtection:KeysDirectory"]));
            }

            return services;
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomErrorHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }

        public static void UseCustomHsts(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
        }

        public static void UseCustomEndpoints(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = HealthCheckResponseWriter.WriteDetailedJsonResponse
                });
            });
        }
    }
}
