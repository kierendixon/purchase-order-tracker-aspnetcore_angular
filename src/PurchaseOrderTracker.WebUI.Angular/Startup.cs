using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PurchaseOrderTracker.Application.Identity;
using PurchaseOrderTracker.WebUI.Angular.Features.Home;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PurchaseOrderTracker.Persistence.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PurchaseOrderTracker.WebUI.Angular
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddFeatureFolders();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(opt =>
            {
                opt.RootPath = "ClientApp/dist/purchase-order-tracker";
            });

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUserHttpContext>();



            //////////////
            ///


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);

                    opt.LoginPath = new PathString("/account"); // challenge path
                    // TODO frontend needs to handle this when performing api calls
                    // or for api calls use a different scheme?
                    //opt.AccessDeniedPath = ""; // forbid path
                    //opt.LogoutPath = ""; // where to redirect after logout

                    opt.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                    };
                });

            services.AddDbContext<Persistence.IdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityDatabase")));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;
            });

            // below is based on
            // https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Identity/Extensions.Core/src/IdentityServiceCollectionExtensions.cs
            // https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Identity/Core/src/IdentityServiceCollectionExtensions.cs

            //// Services identity depends on
            //services.AddOptions().AddLogging();

            //// Services used by identity
            //services.AddScoped<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
            //services.AddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
            //services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
            //services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            //// No interface for the error describer so we can add errors without rev'ing the interface
            //services.AddScoped<IdentityErrorDescriber>();
            //services.AddScoped<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();
            //services.AddScoped<UserManager<ApplicationUser>>();
            //services.AddScoped<IUserStore<ApplicationUser>, UserStore>();
            //services.AddScoped<SignInManager<ApplicationUser>>();

            //////////////






            services.AddDataProtection()
                .SetApplicationName("SharedCookieApp");
                //.PersistKeysToFileSystem("{PATH TO COMMON KEY RING FOLDER}")

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();

            // TODO this app is just hosting the SPA static files. no need for auth
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // TODO main-site
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/main-site/{controller}/{action}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");

                    // alternatively, start the server separately and proxy requests to it
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4201");
                }
            });
        }
    }
}
