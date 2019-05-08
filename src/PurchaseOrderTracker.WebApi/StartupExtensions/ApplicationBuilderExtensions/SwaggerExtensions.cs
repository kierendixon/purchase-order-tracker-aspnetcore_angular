using Microsoft.AspNetCore.Builder;

namespace PurchaseOrderTracker.WebApi.StartupExtensions.ApplicationBuilderExtensions
{
    public static class SwaggerExtensions
    {
        private const string SwaggerSpecUrl = "/swagger/v1/swagger.json";
        private const string SwaggerSpecName = "Purchase Order Tracker API";

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint(SwaggerSpecUrl, SwaggerSpecName);
            });

            return app;
        }
    }
}
