using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PurchaseOrderTracker.Application.Cache;
using PurchaseOrderTracker.Application.Features.Supplier.Commands;

namespace PurchaseOrderTracker.WebApi.Cache;

public class SupplierCacheNotificationHandler : INotificationHandler<CreateCommand.Notification>
{
    private readonly ICacheManager _cacheManager;

    public SupplierCacheNotificationHandler(ICacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }

    Task INotificationHandler<CreateCommand.Notification>.Handle(
        CreateCommand.Notification notification, CancellationToken cancellationToken)
    {
        _cacheManager.RemoveSupplierCache();
        return Task.CompletedTask;
    }
}
