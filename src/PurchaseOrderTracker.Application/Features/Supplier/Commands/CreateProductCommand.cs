using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.Supplier.Commands.CreateProductCommand;

namespace PurchaseOrderTracker.Application.Features.Supplier.Commands;

public class CreateProductCommand : IRequest<Result>
{
    public CreateProductCommand(int supplierId, ProductCode productCode, ProductName productName,
        int categoryId, double? price, Dictionary<int, string> categoryOptions)
    {
        SupplierId = supplierId;
        ProductCode = productCode;
        ProductName = productName;
        CategoryId = categoryId;
        Price = price;
        CategoryOptions = categoryOptions;
    }

    public int SupplierId { get; }
    public ProductCode ProductCode { get; }
    public ProductName ProductName { get; }
    public int CategoryId { get; }
    public double? Price { get; }
    public Dictionary<int, string> CategoryOptions { get; }

    public class Result
    {
        public Result(int supplierId)
        {
            SupplierId = supplierId;
        }

        public int SupplierId { get; }
    }

    public class Handler : IRequestHandler<CreateProductCommand, Result>
    {
        private readonly PoTrackerDbContext _context;
        private readonly IMapper _mapper;

        public Handler(PoTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // TODO await after executing all database queries
                // TODO check if returned supplier and category are found
                var supplier = await _context.Supplier.FindAsync(request.SupplierId);
                var category = _context.ProductCategory.Single(c => c.Id == request.CategoryId);
                var product = _mapper.Map<Product>(request);
                product.Category = category;

                supplier.AddProduct(product);
                await _context.SaveChangesAsync();

                return new Result(request.SupplierId);
            }
            catch (DbUpdateException ex)
            {
                if (ex.IsDuplicateKeyError())
                {
                    throw new PurchaseOrderTrackerException("A duplicate product already exists");
                }

                throw;
            }
        }
    }
}
