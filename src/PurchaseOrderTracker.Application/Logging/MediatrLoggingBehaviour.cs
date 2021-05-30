using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace PurchaseOrderTracker.Application.Logging
{
    public class MediatrLoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public MediatrLoggingBehaviour(ILogger<MediatrLoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // don't call request.ToString() if Debug logging is disabled
            if (_logger.IsEnabled(LogLevel.Debug))
            { 
                _logger.LogDebug("{fullName} [{toString}]", request.GetType().FullName, request.ToString());
            };

            return await next();
        }
    }
}
