using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
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
        private static readonly string _executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // TODO cleanup default config
            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddHeaderPropagation(opt =>
            {
                opt.Headers.Add(HeaderNames.Referer);
                opt.Headers.Add("X-Correlation-ID", ctx => new StringValues(Guid.NewGuid().ToString())); // use Activity class instead
            });

            services.AddHttpClient<PurchaseOrderTrackerHttpClient>(c =>
            {
                c.BaseAddress = new Uri("http://localhost:4202/api/");
                c.DefaultRequestHeaders.Add(HeaderNames.UserAgent, _executingAssemblyName);
            }).AddHeaderPropagation();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(_configureCookies);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", new AuthorizationPolicyBuilder()
                    .RequireClaim(ClaimTypes.Role, "admin")
                    .Build());
            });

            services.AddDataProtection()
                .SetApplicationName("PurchaseOrderTrackerApp");
        }   

        // can alternatively check the request path: context.Request.Path.StartsWithSegments("/api")
        private static bool IsAjaxRequest(HttpRequest request)
        {
            return !(request.Headers.TryGetValue("accept", out var acceptValues)
                && acceptValues.Contains("text/html", StringComparer.InvariantCultureIgnoreCase));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // this must be set before other middleware in the Configure() method so that all requests are logged (including static content)
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseHeaderPropagation();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
