using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;

namespace PurchaseOrderTracker.Web.Features.Api.Shipment
{
    public class Delete
    {
        public class Command : IRequest
        {
            [Required]
            public int? Id { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task Handle(Command command)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var shipment = await _context.Shipment.Include(s => s.Status).SingleAsync(s => s.Id == command.Id);

                if (!shipment.CanBeDeleted)
                    throw new PurchaseOrderTrackerException("Shipment cannot be deleted");

                _context.Shipment.Remove(shipment);
                await _context.SaveChangesAsync();
            }
        }
    }
}