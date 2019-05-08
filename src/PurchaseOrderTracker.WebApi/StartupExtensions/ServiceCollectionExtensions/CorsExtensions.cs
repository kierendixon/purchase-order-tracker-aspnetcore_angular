using Microsoft.Extensions.DependencyInjection;

namespace PurchaseOrderTracker.WebApi.StartupExtensions.ServiceCollectionExtensions
{
    public static class CorsExtensions
    {
        public const string AllowWebServerCorsPolicy = "_allowWebServerCorsPolicy";
        private const string WebServerOrigin = "http://localhost:4200";

        // necessary for clients to discover when authentiction fails due to token expiration
        private const string AuthMethodHeader = "WWW-Authenticate";

        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    AllowWebServerCorsPolicy,
                    builder =>
                    {
                        builder
                            .WithOrigins(WebServerOrigin)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders(AuthMethodHeader)
                            .AllowCredentials();
                    });
            });

            return services;
        }
    }
}
