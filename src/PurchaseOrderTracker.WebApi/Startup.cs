using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        public void ConfigureServices(IServiceCollection services)
        {
            // Application assembly and WebApi assembly
            services.AddAutoMapper(new[] { typeof(CreateCommand), typeof(Startup) });
            services.AddCustomMediatR();
            services.AddCustomIdentity(Configuration);
            services.AddCustomSwagger();
            services.AddCustomMvc();
            services.AddCustomCors();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUserHttpContext>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            services.AddDbContext<PoTrackerDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("PoTrackerDatabase")));

            // This must be configured after services.AddMvc() in ASP.Net 2.1. It is fixed in ASP.Net 2.2
            // https://github.com/aspnet/Docs/issues/6881
            //
            // When the new [ApiController] attribute in ASP.Net Core 2.1 is added to a controller
            // it restricts binding of values from a single source based on a set of inference rules.
            // I.e., values are only bound from query params or only from request body, not both.
            // This breaks binding of values to mediator Command/Query objects which expect values from multiple sources
            // The SuppressInferBindingSourcesForParameters option disables this behaviour.
            // https://github.com/aspnet/Mvc/issues/8111
            //services.Configure<ApiBehaviorOptions>(opt => opt.SuppressInferBindingSourcesForParameters = true);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AutoMapper.IConfigurationProvider autoMapper)
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

            app.UseCors(CorsExtensions.AllowWebServerCorsPolicy);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
