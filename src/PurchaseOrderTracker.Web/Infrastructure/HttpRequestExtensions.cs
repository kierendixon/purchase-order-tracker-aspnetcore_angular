using Microsoft.AspNetCore.Http;

namespace PurchaseOrderTracker.Web.Infrastructure
{
    public static class HttpRequestExtensions
    {
        private const string QueryTypeKey = "querytype";

        public static string GetQueryType(this HttpRequest request)
        {
            return request.Query[QueryTypeKey];
        }
    }
}