using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PurchaseOrderTracker.AspNet.Common.HealthChecks;

namespace PurchaseOrderTracker.WebUI.Angular
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
            services.UseCustomRazorPages();
            services.AddControllersWithViews();
            services.UseCustomSpaStaticFiles(_env);
            services.AddCustomAuthentication();
            services.AddCustomAuthorization();
            services.UseCustomDataProtection(_configuration, _env);
            services.UseCustomHealthChecks(_configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCustomErrorHandler(_env);
            app.UseCustomHsts(_env);
            app.UseStaticFiles();
            app.UseCustomSpaStaticFiles(_env);
            app.UseCustomEndpoints();
            app.UseCustomSpaFallback(_env);
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void UseCustomRazorPages(this IServiceCollection services)
        {
            services.AddRazorPages(opt =>
            {
                opt.RootDirectory = "/Features";
            });
        }

        public static void UseCustomSpaStaticFiles(this IServiceCollection services, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                services.AddSpaStaticFiles(opt =>
                {
                    opt.RootPath = "ClientApp/dist/purchase-order-tracker";
                });
            }
        }

        public static void UseCustomDataProtection(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var dataProtection = services.AddDataProtection()
                .SetApplicationName("PurchaseOrderTrackerApp");

            if (!env.IsDevelopment())
            {
                // not secure - keys will be saved to file system unencrypted
                dataProtection.PersistKeysToFileSystem(new DirectoryInfo(configuration["DataProtection:KeysDirectory"]));
            }
        }

        public static void UseCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddUrlGroup(new Uri(configuration["WebApi.Url"] + "/health"), "PurchaseOrderTracker.WebApi", timeout: TimeSpan.FromSeconds(5));
        }
    }

    // TODO move duplicate code to AspNet.Common
    public static class IdentityServiceCollectionExtensions
    {
        public static void AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(_configureCookies);
        }

        public static void AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator",
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

    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomErrorHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
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

        public static void UseCustomSpaStaticFiles(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
        }

        public static void UseCustomEndpoints(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // TODO controllers are not currently used but I need to change /api direct access to bff pattern
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = HealthCheckResponseWriter.WriteDetailedJsonResponse
                });
            });
        }

        public static void UseCustomSpaFallback(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4201");
                }
            });
        }
    }
}
