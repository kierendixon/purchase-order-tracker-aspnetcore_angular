using System;
using MediatR;

namespace PurchaseOrderTracker.Application.Notifications
{
    public interface ICreateNotification : INotification
    {
        int GetEntityId();
        Type GetEntityType();
    }
}
