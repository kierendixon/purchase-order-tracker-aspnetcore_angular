using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace PurchaseOrderTracker.Application.PagedList;

/// <summary>
/// A minimal list of fields which are required by this application from X.PagedList
/// </summary>
public class PagedListMinimal<T>
{
    // TODO: Move to webAPI layer
    public PagedListMinimal(IPagedList<T> list)
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
