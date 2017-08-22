using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Web.Infrastructure;
using PagedListExtensions = PurchaseOrderTracker.Web.Infrastructure.PagedListExtensions;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    public class Inquiry
    {
        public enum QueryType
        {
            All
        }

        public class Query : IRequest<Result>
        {
            public int PageSize { get; set; } = 5;
            public int PageNumber { get; set; } = 1;

            [Required]
            public QueryType? QueryType { get; set; }
        }

        public class Result
        {
            public Result(PagedListExtensions.PagedListWebApiObject<SupplierViewModel> pagedList)
            {
                PagedList = pagedList;
            }

            public PagedListExtensions.PagedListWebApiObject<SupplierViewModel> PagedList { get; }

            public class SupplierViewModel
            {
                public SupplierViewModel(int id, string name)
                {
                    Id = id;
                    Name = name;
                }

                public int Id { get; }
                public string Name { get; }
            }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Query query)
            {
                var suppliers = _context.Supplier.AsQueryable();

                var paginatedSuppliers = await
                    suppliers.ProjectToPagedList<Result.SupplierViewModel>(query.PageNumber, query.PageSize);

                return new Result(paginatedSuppliers.ToWebApiObject());
            }
        }
    }
}