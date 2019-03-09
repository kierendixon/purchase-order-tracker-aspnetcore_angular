using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using X.PagedList;

namespace PurchaseOrderTracker.Web.Infrastructure
{
    public static class MapperExtensions
    {
        public static async Task<IPagedList<TDestination>> ProjectToPagedList<TDestination>(this IQueryable queryable,
            IConfigurationProvider configuration, int pageNumber, int pageSize)
        {
            return await queryable.ProjectTo<TDestination>(configuration).ToPagedListAsync(pageNumber, pageSize);
        }
    }
}
