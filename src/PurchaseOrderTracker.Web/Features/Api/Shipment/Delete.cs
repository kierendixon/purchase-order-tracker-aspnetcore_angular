using System.ComponentModel.DataAnnotations;
using System.Threading;
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

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            protected override async Task Handle(Command command, CancellationToken cancellationToken)
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