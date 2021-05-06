using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PurchaseOrderTracker.WebUI.Angular
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
            services.UseCustomRazorPages();
            services.AddControllersWithViews();
            services.UseCustomSpaStaticFiles(_env);
            services.UseCustomDataProtection();
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

        public static void UseCustomDataProtection(this IServiceCollection services)
        {
            services.AddDataProtection()
                .SetApplicationName("PurchaseOrderTrackerApp");
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller}/{action}/{id?}");
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
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4201");
                });
            }
        }
    }
}
