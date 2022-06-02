using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Persistence;

namespace PurchaseOrderTracker.Application.Features.Supplier.Commands
{
    public class DeleteCommand : IRequest
    {
        public DeleteCommand(int supplierId)
        {
            SupplierId = supplierId;
        }

        public int SupplierId { get; set; }

        public class Handler : AsyncRequestHandler<DeleteCommand>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
            {
                var supplier = await _context.Supplier.SingleAsync(s => s.Id == request.SupplierId);

                // TODO
                // if (!supplier.CanBeDeleted)
                //    throw new PurchaseOrderTrackerException("Supplier cannot be deleted");
                _context.Supplier.Remove(supplier);
                await _context.SaveChangesAsync();
            }
        }
    }
}
