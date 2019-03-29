using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate;

namespace PurchaseOrderTracker.Web.Features.Api.Shipment
{
    public class EditStatus
    {
        public class Command : IRequest
        {
            [Required]
            [FromRoute]
            public int? ShipmentId { get; set; }

            [Required]
            [FromBody]
            public string UpdatedStatus { get; set; }
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
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var shipment = await _context.Shipment
                    .Include(s => s.PurchaseOrders)
                    .SingleAsync(s => s.Id == command.ShipmentId);

                if (command.UpdatedStatus == ShipmentStatus.Trigger.AwaitingShipping.ToString())
                    shipment.UpdateStatus(ShipmentStatus.Trigger.AwaitingShipping);
                else if (command.UpdatedStatus == ShipmentStatus.Trigger.Shipped.ToString())
                    shipment.UpdateStatus(ShipmentStatus.Trigger.Shipped);
                else if (command.UpdatedStatus == ShipmentStatus.Trigger.Delivered.ToString())
                    shipment.UpdateStatus(ShipmentStatus.Trigger.Delivered);
                else
                    throw new PurchaseOrderTrackerException($"Unexpected update status value '{command.UpdatedStatus}'");

                await _context.SaveChangesAsync();
            }
        }
    }
}
