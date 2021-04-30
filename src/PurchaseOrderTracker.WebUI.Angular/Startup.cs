using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            //services.AddDbContext<Persistence.IdentityDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("IdentityDatabase")));

            services.AddDataProtection()
                .SetApplicationName("PurchaseOrderTrackerApp");
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
