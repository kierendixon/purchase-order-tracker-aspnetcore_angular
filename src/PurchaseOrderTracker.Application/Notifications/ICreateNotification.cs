using System;
using MediatR;

namespace PurchaseOrderTracker.Application.Notifications;

// TODO implement in more commands
public interface ICreateNotification : INotification
{
    int GetEntityId();
    Type GetEntityType();
}
