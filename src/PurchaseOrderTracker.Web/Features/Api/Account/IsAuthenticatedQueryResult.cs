namespace PurchaseOrderTracker.Web.Features.Api.Account
{
    public class IsAuthenticatedQueryResult
    {
        public bool IsAuthenticated { get; }

        public IsAuthenticatedQueryResult(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }
    }
}
