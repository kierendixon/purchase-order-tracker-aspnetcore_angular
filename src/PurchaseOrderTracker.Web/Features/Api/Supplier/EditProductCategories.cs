using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Web.Infrastructure;
using X.PagedList;
using PagedListExtensions = PurchaseOrderTracker.Web.Infrastructure.PagedListExtensions;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    public class EditProductCategories
    {
        public class Query : IRequest<Result>
        {
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 5;

            [Required]
            [FromRoute(Name = "Id")]
            public int? SupplierId { get; set; }
        }

        public class Result
        {

            public Result(int supplierId, string supplierName, PagedListExtensions.PagedListWebApiObject<CategoryViewModel> paginatedCategories)
            {
                Categories = paginatedCategories;
                SupplierId = supplierId;
                SupplierName = supplierName;
            }

            public int SupplierId { get; }
            public string SupplierName { get; }
            public PagedListExtensions.PagedListWebApiObject<CategoryViewModel> Categories { get; }
            
            public class CategoryViewModel
            {
                public CategoryViewModel(int id, string name)
                {
                    Id = id;
                    Name = name;
                }

                public int Id { get; }
                public string Name { get; set; }
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
            {
                var paginatedCategories = await _context.ProductCategory
                    .Where(c => c.SupplierId == query.SupplierId)
                    .ProjectToPagedList<Result.CategoryViewModel>(query.PageNumber, query.PageSize);
                var supplier = await _context.Supplier.SingleAsync(s => s.Id == query.SupplierId);

                return new Result(supplier.Id, supplier.Name, paginatedCategories.ToWebApiObject());
            }
        }
    }
}