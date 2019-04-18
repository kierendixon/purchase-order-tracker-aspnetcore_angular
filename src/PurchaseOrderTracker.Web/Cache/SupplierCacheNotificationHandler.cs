using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PurchaseOrderTracker.Web.Features.Api.Supplier;

namespace PurchaseOrderTracker.Web.Cache
{
    public class SupplierCacheNotificationHandler : INotificationHandler<Create.Notification>
    {
        private readonly ICacheManager _cacheManager;

        public SupplierCacheNotificationHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        Task INotificationHandler<Create.Notification>.Handle(Create.Notification notification, CancellationToken cancellationToken)
        {
            _cacheManager.RemoveSupplierCache();
            return Task.CompletedTask;
        }
    }
}
