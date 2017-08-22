using System.ComponentModel.DataAnnotations;
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
            public int? Id { get; set; }

            [Required]
            [FromBody]
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
                var shipment = await _context.Shipment
                    .Include(s => s.Status)
                    .Include(s => s.PurchaseOrders)
                    .Include(s => s.PurchaseOrders)
                        .ThenInclude(p => p.Status)
                    .SingleAsync(s => s.Id == command.Id);

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