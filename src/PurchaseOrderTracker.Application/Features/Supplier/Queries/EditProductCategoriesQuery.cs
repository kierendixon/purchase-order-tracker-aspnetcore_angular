using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Application.PagedList;
using PurchaseOrderTracker.Persistence;
using X.PagedList;
using static PurchaseOrderTracker.Application.Features.Supplier.Queries.EditProductCategoriesQuery;
using static PurchaseOrderTracker.Application.Features.Supplier.Queries.EditProductCategoriesQuery.Result;
using static PurchaseOrderTracker.Application.PagedList.PagedListExtensions;

namespace PurchaseOrderTracker.Application.Features.Supplier.Queries
{
    public class EditProductCategoriesQuery : IRequest<Result>
    {
        public EditProductCategoriesQuery(int? pageSize, int? pageNumber, int supplierId)
        {
            PageSize = pageSize ?? 5;
            PageNumber = pageNumber ?? 1;
            SupplierId = supplierId;
        }

        public int PageSize { get; }
        public int PageNumber { get; }
        public int SupplierId { get; }

        public class Result
        {
            public Result(int supplierId, string supplierName,
                PagedListMinimal<CategoryViewModel> paginatedCategories)
            {
                Categories = paginatedCategories;
                SupplierId = supplierId;
                SupplierName = supplierName;
            }

            public int SupplierId { get; }
            public string SupplierName { get; }
            public PagedListMinimal<CategoryViewModel> Categories { get; }

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

        public class Handler : IRequestHandler<EditProductCategoriesQuery, Result>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IConfigurationProvider _configuration;
            private readonly IMapper _mapper;

            public Handler(PoTrackerDbContext context, IConfigurationProvider configuration, IMapper mapper)
            {
                _context = context;
                _configuration = configuration;
                _mapper = mapper;
            }

            public async Task<Result> Handle(EditProductCategoriesQuery request, CancellationToken cancellationToken)
            {
                // TODO System.ArgumentException: Expression of type 
                // 'PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects.ProductCategoryName' cannot be 
                // used for constructor parameter of type 'System.String'
                //var paginatedCategories = await _context.ProductCategory
                //    .Where(c => c.SupplierId == request.SupplierId)
                //    .ProjectToPagedList<Result.CategoryViewModel>(_configuration, request.PageNumber, request.PageSize);

                
                var totalCategories = await _context.ProductCategory
                    .Where(c => c.SupplierId == request.SupplierId)
                    .CountAsync();
                var paginatedCategories = await _context.ProductCategory
                    .Where(c => c.SupplierId == request.SupplierId)
                    .OrderBy(c => c.Id)
                    // TODO: The first paginated page of categories loads correctly but queries for 
                    // subsequent pages return 0 product categories instead of the expected amount
                    .Take(request.PageSize)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .ToListAsync();

                var paginatedCategoriesMinimal = new StaticPagedList<CategoryViewModel>(
                    _mapper.Map<List<CategoryViewModel>>(paginatedCategories), request.PageNumber, request.PageSize, totalCategories).ToMinimal();

                // System.ArgumentException: Type 'PurchaseOrderTracker.Application.Features.Supplier.EditProductCategories+Result+CategoryViewModel'
                // does not have a default constructor
                // var paginatedCategories = await _context.Supplier
                //    .Where(s => s.SupplierId == query.SupplierId)
                //    .Select(s => s.ProductCategories)
                //    .ProjectToPagedList<Result.CategoryViewModel>(_configuration, query.PageNumber, query.PageSize);
                var supplier = await _context.Supplier.SingleAsync(s => s.Id == request.SupplierId);

                return new Result(supplier.Id, supplier.Name.Value, paginatedCategoriesMinimal);
            }
        }
    }
}
