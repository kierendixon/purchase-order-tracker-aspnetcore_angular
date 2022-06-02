using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Persistence;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands
{
    public class DeleteCommand : IRequest
    {
        public DeleteCommand(int purchaseOrderId)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public int PurchaseOrderId { get; }

        public class Handler : AsyncRequestHandler<DeleteCommand>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
            {
                var order = await _context.PurchaseOrder.Include(p => p.Status).SingleAsync(p => p.Id == request.PurchaseOrderId);

                if (!order.CanBeDeleted)
                {
                    throw new PurchaseOrderTrackerException("Shipment cannot be deleted");
                }

                _context.PurchaseOrder.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
