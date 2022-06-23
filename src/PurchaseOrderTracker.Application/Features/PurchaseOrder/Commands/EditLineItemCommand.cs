using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands.EditLineItemCommand;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands;

public class EditLineItemCommand : IRequest<Result>
{
    public EditLineItemCommand(int purchaseOrderId, int lineItemId, int productId, int purchaseQty, decimal purchasePrice)
    {
        PurchaseOrderId = purchaseOrderId;
        LineItemId = lineItemId;
        ProductId = productId;
        PurchaseQty = purchaseQty;
        PurchasePrice = purchasePrice;
    }

    public int PurchaseOrderId { get; }
    public int LineItemId { get; }
    public int ProductId { get; }
    public int PurchaseQty { get; }
    public decimal PurchasePrice { get; }

    public class Result
    {
        public Result(int purchaseOrderId)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public int PurchaseOrderId { get; }
    }

    public class Handler : IRequestHandler<EditLineItemCommand, Result>
    {
        private readonly PoTrackerDbContext _context;

        public Handler(PoTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(EditLineItemCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _context.PurchaseOrder
                .Include(p => p.Supplier)
                .Include(p => p.LineItems)
                .ThenInclude(li => li.Product)
                .SingleAsync(o => o.Id == request.PurchaseOrderId);
            var lineItem = purchaseOrder.LineItems.Single(li => li.Id == request.LineItemId);

            await UpdateProductIfChanged(lineItem, request);
            lineItem.PurchasePrice = request.PurchasePrice;
            lineItem.PurchaseQty = request.PurchaseQty;
            await _context.SaveChangesAsync();

            return new Result(request.PurchaseOrderId);
        }

        private async Task UpdateProductIfChanged(PurchaseOrderLine lineItem, EditLineItemCommand command)
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
