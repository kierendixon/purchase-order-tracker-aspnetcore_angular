using Microsoft.EntityFrameworkCore;

namespace PurchaseOrderTracker.Web.Infrastructure
{
    public static class DbUpdateExceptionExtensions
    {
        public static bool IsDuplicateKeyError(this DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                return true;

            return false;
        }
    }
}