using System;

namespace PurchaseOrderTracker.Application.Cache;

// TODO: Make cache duration configurable
[AttributeUsage(AttributeTargets.Class)]
public class CacheAttribute : Attribute
{
    public CacheAttribute(string cacheKey)
    {
        CacheKey = cacheKey;
    }

    public string CacheKey { get; }
}
