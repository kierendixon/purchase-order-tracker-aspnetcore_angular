using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Web.Infrastructure;
using X.PagedList;
using PagedListExtensions = PurchaseOrderTracker.Web.Infrastructure.PagedListExtensions;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    public class EditProducts
    {
        public class Query : IRequest<Result>
        {
            [FromQuery]
            public int PageNumber { get; set; } = 1;

            [FromQuery]
            public int PageSize { get; set; } = 15;

            [Required]
            [FromRoute]
            public int? SupplierId { get; set; }

            [FromQuery]
            public string ProductCodeFilter { get; set; }
        }

        public class Result
        {
            public Result(int supplierId, string supplierName, PagedListExtensions.PagedListWebApiObject<ProductViewModel> products,
                Dictionary<int, string> categoryOptions, bool productsAreFiltered)
            {
                Products = products;
                SupplierId = supplierId;
                SupplierName = supplierName;
                ProductsAreFiltered = productsAreFiltered;
                CategoryOptions = categoryOptions;
            }

            public PagedListExtensions.PagedListWebApiObject<ProductViewModel> Products { get; }
            public int SupplierId { get; }
            public string SupplierName { get; }
            public bool ProductsAreFiltered { get; }
            public Dictionary<int, string> CategoryOptions { get; }

            public class ProductViewModel
            {
                public ProductViewModel(int productId, string prodCode, string name, int? categoryId, decimal? price)
                {
                    ProductId = productId;
                    ProdCode = prodCode;
                    Name = name;
                    Price = price;
                    CategoryId = categoryId;
                }

                public int ProductId { get; }

                [Display(Name = "Product Code")]
                public string ProdCode { get; }

                public string Name { get; }

                [Display(Name = "Category")]
                public int? CategoryId { get; }

                public decimal? Price { get; }
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IMapper _mapper;

            public Handler(PoTrackerDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
            {
                var supplier = await _context.Supplier.Include(s => s.ProductCategories)
                    .SingleAsync(s => s.Id == query.SupplierId);

                if (supplier == null)
                    throw new PurchaseOrderTrackerException($"Cannot find Supplier with id '${query.SupplierId}'");

                IQueryable<Product> products;
                var productsAreFiltered = false;
                if (query.ProductCodeFilter != null)
                {
                    productsAreFiltered = true;
                    products = _context.Product
                        .Include(p => p.Category)
                        .Where(p => p.SupplierId == query.SupplierId
                                && p.ProdCode == query.ProductCodeFilter
                        ).AsQueryable();
                }
                else
                {
                    products = _context.Product
                        .Include(p => p.Category)
                        .Where(p => p.SupplierId == query.SupplierId)
                        .AsQueryable();
                }

                var paginatedProducts =
                    new PagedList<Result.ProductViewModel>(
                        _mapper.Map<IQueryable<Product>, IList<Result.ProductViewModel>>(products),
                        query.PageNumber, query.PageSize);

                // TODO: Can't resolve to Queryable? What happens in MVC project?
//                var paginatedProducts = await
//                    products.ProjectToPagedList<Result.ProductViewModel>(query.PageNumber, query.PageSize);

                return new Result(supplier.Id, supplier.Name, paginatedProducts.ToWebApiObject(),
                    supplier.ProductCategories.ToDictionary(c => c.Id, c => c.Name), productsAreFiltered);
            }
        }
    }
}
