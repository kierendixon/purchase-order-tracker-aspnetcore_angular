using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PurchaseOrderTracker.WebUI.Admin.Logging
{
    // To log outgoing HTTP requests see https://github.com/aspnet/HttpClientFactory/issues/141
    // and https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1#logging
    // and https://www.stevejgordon.co.uk/httpclientfactory-asp-net-core-logging
    // TODO should asp.net log this or instead rely on reverse-proxy logging (i.e., IIS or Azure App Service)?
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogDebug(BuildLogMessage(context).ToString());

            await _next(context);
        }

        // use RecycleableMemoryStream if logging the request Body
        // https://stackoverflow.com/questions/43403941/how-to-read-asp-net-core-response-body/52328142
        private static StringBuilder BuildLogMessage(HttpContext context)
        {
            var sb = new StringBuilder($"INCOMING REQUEST [{context.Request.Method}] {context.Request.Path.Value}");
            sb.Append($"{Environment.NewLine}  Headers:");

            foreach (var header in context.Request.Headers)
            {
                sb.Append($"{Environment.NewLine}  {header.Key}={header.Value}");
            }

            return sb;
        }
    }
}
