using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PurchaseOrderTracker.Application.Cache;

namespace PurchaseOrderTracker.Cache
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly TimeSpan _oneHour = new(1, 0, 0);
        private readonly ILogger<MemoryCacheManager> _logger;
        private readonly IMemoryCache _cache;

        public MemoryCacheManager(IMemoryCache cache, ILogger<MemoryCacheManager> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public Task<TItem> GetOrCreateAsync<TItem>(object key, Func<Task<TItem>> action)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(_oneHour)
                .RegisterPostEvictionCallback(EvictionCallback, this);

            return _cache.GetOrCreateAsync(key, cacheEntry =>
            {
                cacheEntry.SetOptions(cacheEntryOptions);
                return action();
            });
        }

        public void RemoveSupplierCache()
        {
            _cache.Remove(CacheKeys.ShipmentsSummaryResult);
        }

        private void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            _logger.LogDebug($"Cache entry was evicted. Key={key} Reason={reason} State={state}");
        }
    }
}
