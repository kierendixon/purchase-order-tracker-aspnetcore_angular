﻿using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PurchaseOrderTracker.Application.Cache;
using PurchaseOrderTracker.Application.Features.Supplier.Commands;
using PurchaseOrderTracker.Application.Logging;
using PurchaseOrderTracker.AspNet.Common.HealthChecks;
using PurchaseOrderTracker.Cache;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;
using PurchaseOrderTracker.Persistence;
using PurchaseOrderTracker.Persistence.Cache;
using PurchaseOrderTracker.WebApi.Features.User;
using PurchaseOrderTracker.WebApi.Logging;
using PurchaseOrderTracker.WebApi.Mvc;

namespace PurchaseOrderTracker.WebApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        private IConfiguration _configuration { get; }
        private IWebHostEnvironment _env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(new[] {
                typeof(Startup), // WebApi assembly
                typeof(CreateCommand) // Application assembly
            });
            services.AddCustomMediatR();
            services.AddCustomSwagger();
            services.AddCustomAuthentication();
            services.AddCustomAuthorization();
            services.AddCustomControllers();
            services.AddHttpContextAccessor();
            services.AddCustomHealthChecks();
            services.AddMemoryCache();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();

            services.AddDbContext<PoTrackerDbContext>(opt =>
                opt.UseSqlServer(_configuration.GetConnectionString("PoTrackerDatabase")));
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("IdentityDatabase")));

            // TODO
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

            var dataProtection = services.AddDataProtection()
                .SetApplicationName("PurchaseOrderTrackerApp");

            if (!_env.IsDevelopment())
            {
                // not secure - keys will be saved to file system unencrypted
                dataProtection.PersistKeysToFileSystem(new DirectoryInfo(_configuration["DataProtection:KeysDirectory"]));
            }
        }

        public void Configure(IApplicationBuilder app, AutoMapper.IConfigurationProvider autoMapper)
        {
            autoMapper.AssertConfigurationIsValid();

            app.UseCustomErrorHandler(_env);
            app.UseCustomHsts(_env);
            app.UseMiddleware<EnforceRequestHeadersMiddleware>();
            app.UseCustomEndpoints();
            app.UseCustomSwagger(_env);
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(new[]
            {
                typeof(Startup), // WebApi assembly
                typeof(CreateCommand) // Application assembly
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatrLoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatrElapsedTimeBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatrQueryTrackingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatrCacheBehaviour<,>));
        }

        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                var info = new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Purchase Order Tracker API"
                };
                opt.SwaggerDoc("v1", info);

                opt.CustomOperationIds(apiDesc =>
                    apiDesc.ActionDescriptor.RouteValues["controller"]
                    + "_"
                    + (apiDesc.ActionDescriptor.RouteValues["action"] ?? string.Empty)
                    + apiDesc.HttpMethod);

                // disambiguate types with the same name
                opt.CustomSchemaIds(type => type.FullName);
            });
        }

        public static void AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddJsonOptions(opt =>
                {
                    var converters = opt.JsonSerializerOptions.Converters;
                    converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });
        }

        public static void AddCustomHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<PoTrackerDbContext>()
                .AddDbContextCheck<IdentityDbContext>();
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                var swaggerSpecUrl = "/swagger/v1/swagger.json";
                var swaggerSpecName = "Purchase Order Tracker API";

                app.UseSwagger();
                app.UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint(swaggerSpecUrl, swaggerSpecName);
                });
            }
        }

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
                // TODO
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
        }

        // TODO add authentication/authorization
        public static void UseCustomEndpoints(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization();

                endpoints.MapHealthChecks("/health",
                    new HealthCheckOptions()
                    {
                        ResponseWriter = HealthCheckResponseWriter.WriteDetailedJsonResponse
                    }
                );
            });
        }
    }

    // TODO move duplicate code to shared library
    public static class IdentityServiceCollectionExtensions
    {
        public const string RoleAdministrator = "administrator";

        public static void AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(_configureCookies);
        }

        public static void AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(RoleAdministrator,
                    new AuthorizationPolicyBuilder()
                        .RequireClaim(ClaimTypes.Role, "admin")
                        .Build());
            });
        }

        private static readonly Action<CookieAuthenticationOptions> _configureCookies = opt =>
        {
            opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            opt.Cookie.Name = "pot.session";
            opt.LoginPath = new PathString("/account");

            // override default CookieAuthenticationEvents to use different IsAjaxRequest logic
            // https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Security/Authentication/Cookies/src/CookieAuthenticationHandler.cs
            // https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Security/Authentication/Cookies/src/CookieAuthenticationEvents.cs
            opt.Events.OnRedirectToLogin = context =>
            {
                if (IsAjaxRequest(context.Request))
                {
                    //context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
                else
                {
                    context.Response.Redirect(context.RedirectUri);
                }
                return Task.CompletedTask;
            };

            opt.Events.OnRedirectToAccessDenied = context =>
            {
                if (IsAjaxRequest(context.Request))
                {
                    // context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                }
                else
                {
                    context.Response.Redirect(context.RedirectUri);
                }
                return Task.CompletedTask;
            };

            opt.Events.OnRedirectToLogout = context =>
            {
                if (IsAjaxRequest(context.Request))
                {
                    //context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
                }
                else
                {
                    context.Response.Redirect(context.RedirectUri);
                }
                return Task.CompletedTask;
            };

            opt.Events.OnRedirectToReturnUrl = context =>
            {
                if (IsAjaxRequest(context.Request))
                {
                    // context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
                }
                else
                {
                    context.Response.Redirect(context.RedirectUri);
                }
                return Task.CompletedTask;
            };
        };

        // can alternatively check the request path: context.Request.Path.StartsWithSegments("/api")
        private static bool IsAjaxRequest(HttpRequest request)
        {
            return !(request.Headers.TryGetValue("accept", out var acceptValues)
                && acceptValues.Contains("text/html", StringComparer.InvariantCultureIgnoreCase));
        }
    }
}
