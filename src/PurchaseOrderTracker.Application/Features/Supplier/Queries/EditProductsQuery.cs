﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Application.PagedList;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;
using X.PagedList;
using static PurchaseOrderTracker.Application.Features.Supplier.Queries.EditProductsQuery;
using static PurchaseOrderTracker.Application.PagedList.PagedListExtensions;

namespace PurchaseOrderTracker.Application.Features.Supplier.Queries
{
    public class EditProductsQuery : IRequest<Result>
    {
        public EditProductsQuery(int? pageSize, int? pageNumber, int supplierId, ProductCode productCodeFilter)
        {
            PageSize = pageSize ?? 15;
            PageNumber = pageNumber ?? 1;
            SupplierId = supplierId;
            ProductCodeFilter = productCodeFilter;
        }

        public int PageSize { get; }
        public int PageNumber { get; }
        public int SupplierId { get; }
        public ProductCode ProductCodeFilter { get; }

        public class Result
        {
            public Result(int supplierId, SupplierName supplierName, PagedListMinimal<ProductViewModel> products,
                Dictionary<int, string> categoryOptions, bool productsAreFiltered)
            {
                Products = products;
                SupplierId = supplierId;
                SupplierName = supplierName.Value;
                ProductsAreFiltered = productsAreFiltered;
                CategoryOptions = categoryOptions;
            }

            public PagedListMinimal<ProductViewModel> Products { get; }
            public int SupplierId { get; }
            public string SupplierName { get; }
            public bool ProductsAreFiltered { get; }
            public Dictionary<int, string> CategoryOptions { get; }

            // TODO: move to webapi layer?
            public class ProductViewModel
            {
                public ProductViewModel(int productId, string productCode, string name,
                    int? categoryId, decimal? price)
                {
                    ProductId = productId;
                    ProdCode = productCode;
                    Name = name;
                    Price = price;
                    CategoryId = categoryId;
                }

                public int ProductId { get; }
                public string ProdCode { get; }
                public string Name { get; }
                public int? CategoryId { get; }
                public decimal? Price { get; }
            }
        }

        public class Handler : IRequestHandler<EditProductsQuery, Result>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IMapper _mapper;

            public Handler(PoTrackerDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result> Handle(EditProductsQuery request, CancellationToken cancellationToken)
            {
                var supplier = await _context.Supplier.Include(s => s.ProductCategories)
                    .SingleAsync(s => s.Id == request.SupplierId);

                if (supplier == null)
                {
                    throw new PurchaseOrderTrackerException($"Cannot find Supplier with id '${request.SupplierId}'");
                }

                IQueryable<Product> products;
                var productsAreFiltered = false;
                if (request.ProductCodeFilter != null)
                {
                    productsAreFiltered = true;
                    products = _context.Product
                        .Include(p => p.Category)
                        .Where(p => p.SupplierId == request.SupplierId
                        // TODO: how does vlaueobject work for queryable?
                                && p.ProductCode == request.ProductCodeFilter).AsQueryable();
                }
                else
                {
                    products = _context.Product
                        .Include(p => p.Category)
                        .Where(p => p.SupplierId == request.SupplierId)
                        .AsQueryable();
                }

                var paginatedProducts =
                    new PagedList<Result.ProductViewModel>(
                        _mapper.Map<IQueryable<Product>, IList<Result.ProductViewModel>>(products),
                        request.PageNumber, request.PageSize);

                // TODO: Can't resolve to Queryable? What happens in MVC project?
                //                var paginatedProducts = await
                //                    products.ProjectToPagedList<Result.ProductViewModel>(query.PageNumber, query.PageSize);

                return new Result(supplier.Id, supplier.Name, paginatedProducts.ToMinimal(),
                    supplier.ProductCategories.ToDictionary(c => c.Id, c => c.Name.Value), productsAreFiltered);
            }
        }
    }
}