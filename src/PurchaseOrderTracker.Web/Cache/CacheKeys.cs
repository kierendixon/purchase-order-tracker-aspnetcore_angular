using PurchaseOrderTracker.Web.Features.Api.Reporting;

namespace PurchaseOrderTracker.Web.Cache
{
    public static class CacheKeys
    {
        public static string ShipmentsSummary => typeof(ShipmentsSummary).FullName;
    }
}
