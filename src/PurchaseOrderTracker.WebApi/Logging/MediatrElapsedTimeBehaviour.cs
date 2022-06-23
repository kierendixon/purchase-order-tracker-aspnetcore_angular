using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace PurchaseOrderTracker.WebApi.Logging;

public class MediatrElapsedTimeBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<MediatrElapsedTimeBehaviour<TRequest, TResponse>> _logger;

    public MediatrElapsedTimeBehaviour(ILogger<MediatrElapsedTimeBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var watch = Stopwatch.StartNew();
        var response = await next();
        watch.Stop();

        _logger.LogTrace("Type={FullName} ElapsedMs={ElapsedMilliseconds}", request.GetType().FullName, watch.ElapsedMilliseconds);

        return response;
    }
}
