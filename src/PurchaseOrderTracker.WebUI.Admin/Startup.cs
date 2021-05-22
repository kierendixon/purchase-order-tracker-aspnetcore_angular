using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using PurchaseOrderTracker.WebUI.Admin.Controllers;
using PurchaseOrderTracker.WebUI.Admin.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PurchaseOrderTracker.WebUI.Admin
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.UseCustomRazorPages();
            services.UseCustomSpaStaticFiles(_env);
            services.AddCustomHeaderPropagation();
            services.AddCustomHttpClients();
            services.AddCustomAuthentication();
            services.AddCustomAuthorization();
            services.UseCustomDataProtection();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomErrorHandler(env);
            app.UseCustomHsts(env);
            app.UseMiddleware<RequestResponseLoggingMiddleware>(); // log all requests, including static content
            app.UseStaticFiles();
            app.UseCustomSpaStaticFiles(_env);
            app.UseHeaderPropagation();
            app.UseCustomEndpoints();
            app.UseCustomSpaFallback(_env);
        }
    }

    public static class ServiceCollectionExtensions
    {
        private static readonly string _executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        public static void UseCustomRazorPages(this IServiceCollection services)
        {
            services.AddRazorPages(opt =>
            {
                opt.RootDirectory = "/Features"; // TODO error page is still in Pages folder
            });
        }

        public static void UseCustomSpaStaticFiles(this IServiceCollection services, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                services.AddSpaStaticFiles(opt =>
                {
                    opt.RootPath = "ClientApp/build"; ;
                });
            }
        }


        public static void AddCustomHeaderPropagation(this IServiceCollection services)
        {
            services.AddHeaderPropagation(opt =>
            {
                opt.Headers.Add(HeaderNames.Referer);
                // TODO get guid from Activity class instead
                opt.Headers.Add("X-Correlation-ID", ctx => new StringValues(Guid.NewGuid().ToString())); 
            });
        }

        public static void UseCustomDataProtection(this IServiceCollection services)
        {
            services.AddDataProtection()
                .SetApplicationName("PurchaseOrderTrackerApp");
        }

        public static void AddCustomHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<PurchaseOrderTrackerHttpClient>(c =>
            {
                c.BaseAddress = new Uri("http://localhost:4202/api/");
                c.DefaultRequestHeaders.Add(HeaderNames.UserAgent, _executingAssemblyName);
            }).AddHeaderPropagation();
        }
    }

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
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }

        public static void UseCustomSpaFallback(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            // only used for development purposes. in production, SPA files will be served by UseSpaStaticFiles()
            if (env.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.UseReactDevelopmentServer(npmScript: "start");
                });
            }
        }
    }
}
