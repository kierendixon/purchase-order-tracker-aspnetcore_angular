using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;

namespace PurchaseOrderTracker.Application.Features.Shipment.Commands
{
    public class EditStatusCommand : IRequest
    {
        public EditStatusCommand(int shipmentId, ShipmentStatus.Trigger updatedStatus)
        {
            ShipmentId = shipmentId;
            UpdatedStatus = updatedStatus;
        }

        public int ShipmentId { get; }

        public ShipmentStatus.Trigger UpdatedStatus { get; }

        public class Handler : AsyncRequestHandler<EditStatusCommand>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(EditStatusCommand request, CancellationToken cancellationToken)
            {
                var shipment = await _context.Shipment
                    .Include(s => s.PurchaseOrders)
                    .SingleAsync(s => s.Id == request.ShipmentId);

                switch (request.UpdatedStatus)
                {
                    case ShipmentStatus.Trigger.AwaitingShipping:
                        shipment.UpdateStatus(ShipmentStatus.Trigger.AwaitingShipping);
                        break;
                    case ShipmentStatus.Trigger.Shipped:
                        shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);
                        break;
                    case ShipmentStatus.Trigger.Delivered:
                        shipment.UpdateStatus(ShipmentStatus.Trigger.Delivered);
                        break;
                    default:
                        throw new PurchaseOrderTrackerException($"Unexpected update status value '{request.UpdatedStatus}'");
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
