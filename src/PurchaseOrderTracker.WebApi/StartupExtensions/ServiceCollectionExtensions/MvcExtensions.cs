using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace PurchaseOrderTracker.WebApi.StartupExtensions.ServiceCollectionExtensions
{
    public static class MvcExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            // need full mvc (instead of AddMvcCore) for Swagger UI
            services.AddMvc(opt =>
            {
                // require authenticated user by default
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            return services;
        }
    }
}
