using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Persistence;

namespace PurchaseOrderTracker.Application.Features.Supplier.Commands
{
    public class DeleteProductCategoryCommand : IRequest
    {
        public DeleteProductCategoryCommand(int supplierId, int categoryId)
        {
            SupplierId = supplierId;
            CategoryId = categoryId;
        }

        public int SupplierId { get; }
        public int? CategoryId { get; }

        public class Handler : AsyncRequestHandler<DeleteProductCategoryCommand>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
            {
                var supplier = await _context.Supplier.FindAsync(request.SupplierId);
                var category = await _context.Entry(supplier).Collection(s => s.ProductCategories).Query()
                    .Where(c => c.Id == request.CategoryId).SingleAsync();

                supplier.RemoveCategory(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
