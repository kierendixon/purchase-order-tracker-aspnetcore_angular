using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    public class EditLineItem
    {
        public class Command : IRequest<Result>
        {
            [Required]
            public int? PurchaseOrderId { get; set; }

            [Required]
            public int? LineItemId { get; set; }

            [Required]
            public int? ProductId { get; set; }

            [Required]
            public int? PurchaseQty { get; set; }

            [Required]
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

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var purchaseOrder = await _context.PurchaseOrder
                    .Include(p => p.Supplier)
                    .Include(p => p.LineItems)
                    .ThenInclude(li => li.Product)
                    .SingleAsync(o => o.Id == command.PurchaseOrderId);
                var lineItem = purchaseOrder.LineItems.Single(li => li.Id == command.LineItemId);

                await UpdateProductIfChanged(lineItem, command);
                lineItem.PurchasePrice = command.PurchasePrice.Value;
                lineItem.PurchaseQty = command.PurchaseQty.Value;
                await _context.SaveChangesAsync();

                return new Result(command.PurchaseOrderId.Value);
            }

            private async Task UpdateProductIfChanged(PurchaseOrderLine lineItem, Command command)
            {
                if (lineItem.Product.Id != command.ProductId)
                {
                    var newProduct = await _context.Product.SingleAsync(p => p.SupplierId == lineItem.Product.SupplierId
                                                      && p.Id == command.ProductId);
                    lineItem.Product = newProduct;
                }
            }
        }
    }
}