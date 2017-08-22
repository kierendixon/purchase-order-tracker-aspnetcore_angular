using Microsoft.AspNetCore.Routing;

namespace PurchaseOrderTracker.Web.Infrastructure
{
    public static class RouteDataExtensions
    {
        private const string ControllerKey = "controller";
        private const string ActionKey = "action";

        public static string GetController(this RouteData routeData)
        {
            return routeData.Values[ControllerKey].ToString();
        }

        public static string GetAction(this RouteData routeData)
        {
            return routeData.Values[ActionKey].ToString();
        }
    }
}