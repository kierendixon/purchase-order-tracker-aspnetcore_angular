using System.ComponentModel.DataAnnotations;
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

        public class CommandHandler : IAsyncRequestHandler<Command>
        {
            private readonly PoTrackerDbContext _context;

            public CommandHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task Handle(Command command)
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