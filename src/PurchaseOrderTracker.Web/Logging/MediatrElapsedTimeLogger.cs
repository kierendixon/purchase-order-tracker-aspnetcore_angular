using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace PurchaseOrderTracker.Web.Logging
{
    public class MediatrElapsedTimeLogger<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<MediatrElapsedTimeLogger<TRequest, TResponse>> _logger;

        public MediatrElapsedTimeLogger(ILogger<MediatrElapsedTimeLogger<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var watch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid();
            _logger.LogTrace($"Handling request. Id={requestId} Type={request.GetType().FullName}");

            var response = await next();

            watch.Stop();
            _logger.LogTrace($"Finished. Id={requestId} ElapsedMs={watch.ElapsedMilliseconds}");

            return response;
        }
    }
}
