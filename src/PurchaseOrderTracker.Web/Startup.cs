using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Web.Identity;

namespace PurchaseOrderTracker.Web
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
            services.AddAutoMapper(typeof(Startup));

            services.AddMediatR(typeof(Startup));

            AddIdentityServices(services);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFeatureFolders();

            // This must be configured after services.AddMvc() in ASP.Net 2.1. It is fixed in ASP.Net 2.2
            // https://github.com/aspnet/Docs/issues/6881
            //
            // When the new [ApiController] attribute in ASP.Net Core 2.1 is added to a controller
            // it restricts binding of values from a single source based on a set of inference rules.
            // I.e., values are only bound from query params or only from request body, not both.
            // This breaks binding of values to mediator Command/Query objects which expect values from multiple sources
            // The SuppressInferBindingSourcesForParameters option disables this behaviour.
            // https://github.com/aspnet/Mvc/issues/8111
            services.Configure<ApiBehaviorOptions>(opt => opt.SuppressInferBindingSourcesForParameters = true);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddDbContext<PoTrackerDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PoTrackerDatabase")));

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AutoMapper.IConfigurationProvider autoMapper)
        {
            autoMapper.AssertConfigurationIsValid();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4201");
                }
            });
        }

        private void AddIdentityServices(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityDatabase")));
        }
    }
}
