using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands.CreateLineItemCommand;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands;

public class CreateLineItemCommand : IRequest<Result>
{
    public CreateLineItemCommand(int purchaseOrderId, int productId, int purchaseQty, decimal? purchasePrice)
    {
        PurchaseOrderId = purchaseOrderId;
        ProductId = productId;
        PurchaseQty = purchaseQty;
        PurchasePrice = purchasePrice;
    }

    public int PurchaseOrderId { get; }
    public int ProductId { get; }
    public int PurchaseQty { get; }
    public decimal? PurchasePrice { get; }

    public class Result
    {
        public Result(int purchaseOrderId)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public int PurchaseOrderId { get; }
    }

    public class Handler : IRequestHandler<CreateLineItemCommand, Result>
    {
        private readonly PoTrackerDbContext _context;

        public Handler(PoTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateLineItemCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _context.PurchaseOrder
                .Include(p => p.Supplier)
                .SingleAsync(o => o.Id == request.PurchaseOrderId);

            // TODO: throw exception is product is null?
            var product = await _context.Product.FindAsync(request.ProductId);
            var lineItem = new PurchaseOrderLine(product, request.PurchasePrice.Value, request.PurchaseQty);

            purchaseOrder.AddLineItem(lineItem);
            await _context.SaveChangesAsync();

            return new Result(request.PurchaseOrderId);
        }
    }
}
