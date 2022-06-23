using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using X.PagedList;

namespace PurchaseOrderTracker.Application.PagedList;

public static class PagedListExtensions
{
    // TODO: Move to WebAPI layer
    public static PagedListMinimal<T> ToMinimal<T>(this IPagedList<T> list)
    {
        return new PagedListMinimal<T>(list);
    }

    public static async Task<IPagedList<TDestination>> ProjectToPagedList<TDestination>(
        this IQueryable queryable, IConfigurationProvider configuration, int pageNumber, int pageSize)
    {
        return await queryable.ProjectTo<TDestination>(configuration).ToPagedListAsync(pageNumber, pageSize);
    }
}
