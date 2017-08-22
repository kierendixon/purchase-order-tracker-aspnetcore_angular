using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using X.PagedList;

namespace PurchaseOrderTracker.Web.Infrastructure
{
    public static class MapperExtensions
    {
        public static async Task<IPagedList<TDestination>> ProjectToPagedList<TDestination>(this IQueryable queryable,
            int pageNumber, int pageSize)
        {
            return await queryable.ProjectTo<TDestination>().ToPagedListAsync(pageNumber, pageSize);
        }
    }
}