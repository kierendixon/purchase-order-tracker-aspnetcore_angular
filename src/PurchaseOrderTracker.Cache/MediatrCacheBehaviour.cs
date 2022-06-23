using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PurchaseOrderTracker.Application.Cache;

namespace PurchaseOrderTracker.Cache;

public class MediatrCacheBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ICacheManager _cache;

    public MediatrCacheBehaviour(ICacheManager cacheManager)
    {
        _cache = cacheManager;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "code becomes harder to read")]
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var cacheAttribute = typeof(TResponse).GetTypeInfo().GetCustomAttribute<CacheAttribute>();
        if (cacheAttribute != null)
        {
            return await _cache.GetOrCreateAsync(cacheAttribute.CacheKey, async () => await next());
        }

        return await next();
    }
}
