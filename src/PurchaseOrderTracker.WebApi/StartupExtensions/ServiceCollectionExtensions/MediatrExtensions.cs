using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOrderTracker.Application.Features.Supplier.Commands;
using PurchaseOrderTracker.Cache;
using PurchaseOrderTracker.Persistence;
using PurchaseOrderTracker.WebApi.Logging;

namespace PurchaseOrderTracker.WebApi.StartupExtensions.ServiceCollectionExtensions
{
    public static class MediatrExtensions
    {
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(new[]
            {
                typeof(Startup), // WebApi assembly
                typeof(CreateCommand) // Application assembly
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatrElapsedTimeBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatrQueryTrackingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatrCacheBehaviour<,>));

            return services;
        }
    }
}
