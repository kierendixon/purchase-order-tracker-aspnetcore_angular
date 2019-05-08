using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands
{
    public class EditStatusCommand : IRequest
    {
        public EditStatusCommand(int purchaseOrderId, PurchaseOrderStatus.Trigger updatedStatus)
        {
            PurchaseOrderId = purchaseOrderId;
            UpdatedStatus = updatedStatus;
        }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrderStatus.Trigger UpdatedStatus { get; set; }

        public class Handler : AsyncRequestHandler<EditStatusCommand>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(EditStatusCommand request, CancellationToken cancellationToken)
            {
                var order = await _context.PurchaseOrder.Include(p => p.Status).SingleAsync(p => p.Id == request.PurchaseOrderId);

                switch (request.UpdatedStatus)
                {
                    case PurchaseOrderStatus.Trigger.PendingApproval:
                        order.UpdateStatus(PurchaseOrderStatus.Trigger.PendingApproval);
                        break;
                    case PurchaseOrderStatus.Trigger.Approved:
                        order.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                        break;
                    case PurchaseOrderStatus.Trigger.Cancelled:
                        order.UpdateStatus(PurchaseOrderStatus.Trigger.Cancelled);
                        break;
                    default:
                        throw new PurchaseOrderTrackerException($"Unexpected update status value '{request.UpdatedStatus}'");
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
