using System;
using MediatR;

namespace PurchaseOrderTracker.Web.Features.Notifications
{
    public interface ICreateNotification : INotification
    {
        int GetEntityId();
        Type GetEntityType();
    }
}
