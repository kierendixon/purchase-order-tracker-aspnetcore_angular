using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;

namespace PurchaseOrderTracker.Application.Features.Supplier.Commands
{
    public class CreateProductCategoryCommand : IRequest
    {
        public CreateProductCategoryCommand(int supplierId, ProductCategoryName name)
        {
            SupplierId = supplierId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int SupplierId { get; }
        public ProductCategoryName Name { get; }

        public class Handler : AsyncRequestHandler<CreateProductCategoryCommand>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var supplier = await _context.Supplier.FindAsync(request.SupplierId);
                    supplier.AddCategory(new ProductCategory(request.Name));

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.IsDuplicateKeyError())
                    {
                        throw new PurchaseOrderTrackerException("A duplicate product category already exists");
                    }

                    throw;
                }
            }
        }
    }
}
