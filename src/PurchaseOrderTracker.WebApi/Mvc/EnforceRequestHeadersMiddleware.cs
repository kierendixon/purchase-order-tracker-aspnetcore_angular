using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PurchaseOrderTracker.WebApi.Mvc
{
    // TODO for a more robust solution see
    // https://github.com/khellang/Middleware/blob/master/src/ProblemDetails/ProblemDetailsMiddleware.cs
    // https://github.com/aspnet/Mvc/issues/7238
    // or
    // https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-5.0#exception-handler
    // ^ see [Route("/error-local-development")]
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
            //var containsClientId = context.Request.Headers.ContainsKey(ClientIdHeader);
            //var containsTraceId = context.Request.Headers.ContainsKey(TraceIdHeader);

            await _next(context);

            // if (containsClientId && containsTraceId)
            // {
            //    await _next(context);
            // }
            // else
            // {
            //    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    // TODO use json settings configured for the ASP.NET app
            //    await context.Response.WriteAsJsonAsync(BuildProblemDetails(context, containsClientId, containsTraceId));
            // }
        }

        // TODO should this instead get the DefaultProblemDetailsFactory from the DI container and use it to
        // create a problemdetails, similar to MVC's ControllerBase? The factory takes a ModelStateDictionary
        // https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.Core/src/Infrastructure/DefaultProblemDetailsFactory.cs
        //private ValidationProblemDetails BuildProblemDetails(HttpContext context, bool containsClientId, bool containsTraceId)
        //{
        //    var missingHeaders = new string[containsClientId || containsTraceId ? 1 : 2];
        //    if (!containsClientId)
        //    {
        //        missingHeaders[0] = $"{ClientIdHeader} header is required";
        //    }

        //    if (!containsTraceId)
        //    {
        //        missingHeaders[containsClientId ? 0 : 1] = $"{TraceIdHeader} header is required";
        //    }

        //    var errors = new Dictionary<string, string[]>()
        //    {
        //        { string.Empty, missingHeaders }
        //    };

        //    var problemDetails = new ValidationProblemDetails(errors)
        //    {
        //        Title = "Missing headers",
        //        Status = (int)HttpStatusCode.BadRequest,
        //    };

        //    // from https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.Core/src/Infrastructure/DefaultProblemDetailsFactory.cs
        //    var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;
        //    if (traceId != null)
        //    {
        //        problemDetails.Extensions["traceId"] = traceId;
        //    }

        //    return problemDetails;
        //}
    }
}
