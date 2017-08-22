using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    public class EditStatus
    {
        public class Command : IRequest
        {
            [Required]
            public int? Id { get; set; }

            [Required]
            public string UpdatedStatus { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command>
        {
            private readonly PoTrackerDbContext _context;

            public CommandHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task Handle(Command command)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var order = await _context.PurchaseOrder.Include(p => p.Status).SingleAsync(p => p.Id == command.Id);

                if (command.UpdatedStatus == PurchaseOrderStatus.Trigger.PendingApproval.ToString())
                    order.UpdateStatus(PurchaseOrderStatus.Trigger.PendingApproval);
                else if (command.UpdatedStatus == PurchaseOrderStatus.Trigger.Approved.ToString())
                    order.UpdateStatus(PurchaseOrderStatus.Trigger.Approved);
                else if (command.UpdatedStatus == PurchaseOrderStatus.Trigger.Cancelled.ToString())
                    order.UpdateStatus(PurchaseOrderStatus.Trigger.Cancelled);
                else
                    throw new PurchaseOrderTrackerException($"Unexpected update status value '{command.UpdatedStatus}'");

                await _context.SaveChangesAsync();
            }
        }
    }
}