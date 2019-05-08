using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;

namespace PurchaseOrderTracker.Application.Features.Supplier.Commands
{
    public class EditProductCategoryCommand : IRequest
    {
        public EditProductCategoryCommand(int supplierId, int categoryId, ProductCategoryName name)
        {
            SupplierId = supplierId;
            CategoryId = categoryId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int SupplierId { get; }
        public int CategoryId { get; }
        public ProductCategoryName Name { get; }

        public class Handler : AsyncRequestHandler<EditProductCategoryCommand>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(EditProductCategoryCommand request, CancellationToken cancellationToken)
            {
                var supplier = await _context.Supplier.FindAsync(request.SupplierId);
                var category = await _context.Entry(supplier).Collection(s => s.ProductCategories).Query()
                    .Where(c => c.Id == request.CategoryId).SingleAsync();

                category.Name = request.Name;
                await _context.SaveChangesAsync();
            }
        }
    }
}
