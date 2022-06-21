using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PurchaseOrderTracker.Application.Notifications;

namespace PurchaseOrderTracker.WebApi.Logging
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
            _logger.LogDebug("Created {Name} Id={EntityId}", notification.GetEntityType().Name, notification.GetEntityId());
            return Task.CompletedTask;
        }
    }
}
