using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Persistence;

namespace PurchaseOrderTracker.Application.Features.Shipment.Commands
{
    public class DeleteCommand : IRequest
    {
        public DeleteCommand(int shipmentId)
        {
            ShipmentId = shipmentId;
        }

        public int ShipmentId { get; set; }

        public class Handler : AsyncRequestHandler<DeleteCommand>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
            {
                var shipment = await _context.Shipment.SingleAsync(s => s.Id == request.ShipmentId);
                ThrowExceptionIfCannotBeDeleted(shipment);
                _context.Shipment.Remove(shipment);

                await _context.SaveChangesAsync();
            }

            private void ThrowExceptionIfCannotBeDeleted(Domain.Models.ShipmentAggregate.Shipment shipment)
            {
                if (!shipment.CanBeDeleted)
                {
                    throw new PurchaseOrderTrackerException("Shipment cannot be deleted");
                }
            }
        }
    }
}
