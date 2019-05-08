using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace PurchaseOrderTracker.WebApi.StartupExtensions.ServiceCollectionExtensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                var info = new Info
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

            return services;
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
}
