using System;
using Microsoft.EntityFrameworkCore;

namespace PurchaseOrderTracker.Persistence;

public static class DbUpdateExceptionExtensions
{
    public static bool IsDuplicateKeyError(this DbUpdateException ex)
    {
        return ex.InnerException != null
            && ex.InnerException.Message.Contains("Cannot insert duplicate key row", StringComparison.Ordinal);
    }
}
