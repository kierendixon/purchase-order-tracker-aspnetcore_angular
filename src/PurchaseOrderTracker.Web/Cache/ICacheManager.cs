using System;
using System.Threading.Tasks;

namespace PurchaseOrderTracker.Web.Cache
{
    public interface ICacheManager
    {
        void RemoveSupplierCache();
        Task<TItem> GetOrCreateAsync<TItem>(object key, Func<Task<TItem>> action);
    }
}
