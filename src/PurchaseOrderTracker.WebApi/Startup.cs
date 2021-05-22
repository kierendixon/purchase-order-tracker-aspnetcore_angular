using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PurchaseOrderTracker.Application.Cache;
using PurchaseOrderTracker.Application.Features.Supplier.Commands;
using PurchaseOrderTracker.Cache;
using PurchaseOrderTracker.Persistence;
using PurchaseOrderTracker.Persistence.Cache;
using PurchaseOrderTracker.WebApi.Logging;
using PurchaseOrderTracker.WebApi.Mvc;

namespace PurchaseOrderTracker.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env )
        {
            Configuration = configuration;
            _env = env;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment _env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Application assembly and WebApi assembly
            services.AddAutoMapper(new[] { typeof(CreateCommand), typeof(Startup) });
            services.AddCustomMediatR();
            // TODO add authN/authZ
            //services.AddCustomIdentity(Configuration);
            services.AddCustomSwagger();

            services.AddControllersWithViews()
                .AddJsonOptions(opt =>
                {
                    var converters = opt.JsonSerializerOptions.Converters;
                    converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });

            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            services.AddDbContext<PoTrackerDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("PoTrackerDatabase")));
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

                // Fix names for generic types
                // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/752#issuecomment-467817189
                opt.CustomSchemaIds(type => DefaultSchemaIdSelector(type));
            });
        }

        private static string DefaultSchemaIdSelector(Type modelType)
        {
            string schemaId;

            if (!modelType.IsConstructedGenericType)
            {
                schemaId = modelType.FullName;
            }
            else
            {
                var prefix = modelType.GetGenericArguments()
                    .Select(genericArg => DefaultSchemaIdSelector(genericArg))
                    .Aggregate((previous, current) => previous + current);

                schemaId = prefix + modelType.Name.Split('`').First();
            }

            return schemaId.Replace("+", string.Empty);
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

        public static void UseCustomEndpoints(this IApplicationBuilder app)
        {
            app.UseRouting();
            // TODO
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                    //.RequireAuthorization();
            });
        }
    }
}
