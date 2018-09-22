using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    public class Delete
    {
        public class Command : IRequest
        {
            [Required]
            public int? Id { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly PoTrackerDbContext _context;

            public CommandHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(Command command, CancellationToken cancellationToken)
            {
                var order = await _context.PurchaseOrder.Include(p => p.Status).SingleAsync(p => p.Id == command.Id);

                if (!order.CanBeDeleted)
                    throw new PurchaseOrderTrackerException("Shipment cannot be deleted");

                _context.PurchaseOrder.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}