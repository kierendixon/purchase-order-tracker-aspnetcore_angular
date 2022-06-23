using System;

namespace PurchaseOrderTracker.Domain.Exceptions;

public class PurchaseOrderTrackerException : Exception
{
    public PurchaseOrderTrackerException(string message)
        : base(message)
    {
    }

    public PurchaseOrderTrackerException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
