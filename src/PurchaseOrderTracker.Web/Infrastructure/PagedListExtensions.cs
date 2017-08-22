using System.Collections;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace PurchaseOrderTracker.Web.Infrastructure
{
    public static class PagedListExtensions
    {
        public static PagedListWebApiObject<T> ToWebApiObject<T>(this IPagedList<T> list)
        {
            return new PagedListWebApiObject<T>(list);
        }

        public class PagedListWebApiObject<T>
        {
            public PagedListWebApiObject(IPagedList<T> list)
            {
                HasNextPage = list.HasNextPage;
                HasPreviousPage = list.HasPreviousPage;
                PageSize = list.PageSize;
                PageNumber = list.PageNumber;
                PageCount = list.PageCount;
                Items = list.ToList();
            }

            public bool HasNextPage { get; }
            public bool HasPreviousPage { get; }
            public int PageSize { get; }
            public int PageNumber { get; }
            public int PageCount { get; }
            public List<T> Items { get; }
        }
    }
}