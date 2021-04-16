using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using PurchaseOrderTracker.WebUI.Admin.Controllers;
using PurchaseOrderTracker.WebUI.Admin.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;

namespace PurchaseOrderTracker.WebUI.Admin
{
    public class Startup
    {
        private static readonly string _executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

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




            /////
            ///

            services.AddAuthentication("Cookies")
                .AddCookie(opt =>
                {
                    opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    opt.Cookie.Name = "pot.session";
                    opt.LoginPath = new PathString("/account");
                });

            services.AddDataProtection()
                .SetApplicationName("SharedCookieApp");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrators", new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.Role, "admin")
                    .Build());
            });
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
