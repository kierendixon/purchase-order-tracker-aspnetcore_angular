using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.WebApi.Mvc
{
    public class EnforceRequestHeadersMiddleware
    {
        // TODO which headers should we check? correlationId?
        public const string ClientIdHeader = "clientid";
        public const string TraceIdHeader = "traceid";

        private readonly RequestDelegate _next;

        public EnforceRequestHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var containsId = context.Request.Headers.ContainsKey(ClientIdHeader);
            var containsTraceId = context.Request.Headers.ContainsKey(TraceIdHeader);

            if (containsId && containsTraceId)
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(BuildProblemDetails(context, containsId, containsTraceId));
            }
        }

        private ValidationProblemDetails BuildProblemDetails(HttpContext context, bool containsId, bool containsTraceId)
        {
            var missingHeaders = new string[containsId || containsTraceId ? 1 : 2];
            if (!containsId)
            {
                missingHeaders[0] = $"{ClientIdHeader} header is required";
            }

            if (!containsTraceId)
            {
                missingHeaders[containsId ? 0 : 1] = $"{TraceIdHeader} header is required";
            }

            var errors = new Dictionary<string, string[]>()
            {
                { string.Empty, missingHeaders }
            };

            var problemDetails = new ValidationProblemDetails(errors)
            {
                Title = "Missing headers",
                Status = (int)HttpStatusCode.BadRequest
            };

            // from https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.Core/src/Infrastructure/DefaultProblemDetailsFactory.cs
            var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;
            if (traceId != null)
            {
                problemDetails.Extensions["traceId"] = traceId;
            }

            return problemDetails;
        }
    }
}
