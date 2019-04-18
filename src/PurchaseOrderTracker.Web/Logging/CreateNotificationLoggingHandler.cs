using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PurchaseOrderTracker.Web.Features.Notifications;

namespace PurchaseOrderTracker.Web.Cache
{
    public class CreateNotificationLoggingHandler : INotificationHandler<ICreateNotification>
    {
        private readonly ILogger<CreateNotificationLoggingHandler> _logger;

        public CreateNotificationLoggingHandler(ILogger<CreateNotificationLoggingHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ICreateNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Created object. Type={notification.GetEntityType().Name} Id={notification.GetEntityId()}");
            return Task.CompletedTask;
        }
    }
}
