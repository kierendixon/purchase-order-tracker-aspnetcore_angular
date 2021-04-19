using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PurchaseOrderTracker.Application.Cache;
using PurchaseOrderTracker.Application.Features.Supplier.Commands;
using PurchaseOrderTracker.Application.Identity;
using PurchaseOrderTracker.Persistence;
using PurchaseOrderTracker.Persistence.Cache;
using PurchaseOrderTracker.WebApi.Identity;
using PurchaseOrderTracker.WebApi.StartupExtensions.ApplicationBuilderExtensions;
using PurchaseOrderTracker.WebApi.StartupExtensions.ServiceCollectionExtensions;

namespace PurchaseOrderTracker.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // TODO: add middleware to validate (and return bad request if not valid)
        // that mandatory headers are provided (user agent, correlation id)
        public void ConfigureServices(IServiceCollection services)
        {
            // Application assembly and WebApi assembly
            services.AddAutoMapper(new[] { typeof(CreateCommand), typeof(Startup) });
            services.AddCustomMediatR();
            services.AddCustomIdentity(Configuration);
            services.AddCustomSwagger();

            services.AddControllersWithViews()
                .AddJsonOptions(opt =>
                {
                    var converters = opt.JsonSerializerOptions.Converters;
                    converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });

            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUserHttpContext>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            services.AddDbContext<PoTrackerDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("PoTrackerDatabase")));

            // TODO use ApiController
            // Note below is from when this application was using ASP.NET 2.1...
            // When the new [ApiController] attribute in ASP.Net Core 2.1 is added to a controller
            // it restricts binding of values from a single source based on a set of inference rules.
            // I.e., values are only bound from query params or only from request body, not both.
            // This breaks binding of values to mediator Command/Query objects which expect values from multiple sources
            // The SuppressInferBindingSourcesForParameters option disables this behaviour.
            // https://github.com/aspnet/Mvc/issues/8111
            //services.Configure<ApiBehaviorOptions>(opt => opt.SuppressInferBindingSourcesForParameters = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AutoMapper.IConfigurationProvider autoMapper)
        {
            autoMapper.AssertConfigurationIsValid();

            if (env.IsDevelopment())
            {
                app.UseCustomSwagger();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllerRoute(
                        name: "default",
                        pattern: "{controller}/{action=Index}/{id?}")
                    .RequireAuthorization();
            });

        }
    }
}
