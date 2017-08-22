using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    public class CreateLineItem
    {
        public class Command : IRequest<Result>
        {
            [Required]
            public int? PurchaseOrderId { get; set; }

            [Required]
            public int? ProductId { get; set; }

            [Required]
            public int? PurchaseQty { get; set; }

            public decimal? PurchasePrice { get; set; }
        }

        public class Result
        {
            public Result(int purchaseOrderId)
            {
                PurchaseOrderId = purchaseOrderId;
            }

            public int PurchaseOrderId { get; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Result>
        {
            private readonly PoTrackerDbContext _context;

            public CommandHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Command command)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var purchaseOrder = await _context.PurchaseOrder
                    .Include(p => p.Supplier)
                    .SingleAsync(o => o.Id == command.PurchaseOrderId);
                var product = await _context.Product.FindAsync(command.ProductId);
                var lineItem = new PurchaseOrderLine(product, command.PurchasePrice.Value, command.PurchaseQty.Value);

                purchaseOrder.AddLineItem(lineItem);
                await _context.SaveChangesAsync();

                return new Result(command.PurchaseOrderId.Value);
            }
        }
    }
}