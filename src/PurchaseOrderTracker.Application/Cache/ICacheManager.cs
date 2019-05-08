using System;
using System.Threading.Tasks;

namespace PurchaseOrderTracker.Application.Cache
{
    public interface ICacheManager
    {
        Task<TItem> GetOrCreateAsync<TItem>(object key, Func<Task<TItem>> action);
        void RemoveSupplierCache();
    }
}
