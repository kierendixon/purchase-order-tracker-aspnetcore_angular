using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PurchaseOrderTracker.Application.Cache;
using PurchaseOrderTracker.Application.Features.Supplier.Commands;
using PurchaseOrderTracker.AspNet.Common.HealthChecks;
using PurchaseOrderTracker.Cache;
using PurchaseOrderTracker.Identity.Persistence;
using PurchaseOrderTracker.Persistence;
using PurchaseOrderTracker.Persistence.Cache;
using PurchaseOrderTracker.WebApi.Logging;
using PurchaseOrderTracker.WebApi.Mvc;

namespace PurchaseOrderTracker.WebApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment _env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(new[] {
                typeof(Startup), // WebApi assembly
                typeof(CreateCommand) // Application assembly
            });
            services.AddCustomMediatR();
            services.AddCustomSwagger();
            services.AddCustomControllers();
            services.AddHttpContextAccessor();
            services.AddCustomHealthChecks();
            services.AddMemoryCache();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();

            services.AddDbContext<PoTrackerDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("PoTrackerDatabase")));
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityDatabase")));
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
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //.RequireAuthorization();
                endpoints.MapHealthChecks("/health",
                    new HealthCheckOptions()
                    {
                        ResponseWriter = HealthCheckResponseWriter.WriteDetailedJsonResponse
                    }
                );
            });
        }
    }
}
