using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries.EditLineItemsQuery;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries
{
    public class EditLineItemsQuery : IRequest<Result>
    {
        public EditLineItemsQuery(int purchaseOrderId)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public int PurchaseOrderId { get; }

        public class Result
        {
            public Result(int purchaseOrderId, OrderNo purchaseOrderOrderNo,
                IEnumerable<PurchaseOrderLine> lineItems, Dictionary<int, ProductName> productOptions)
            {
                PurchaseOrderId = purchaseOrderId;
                PurchaseOrderOrderNo = purchaseOrderOrderNo;
                LineItems = lineItems;
                ProductOptions = productOptions;
            }

            public int PurchaseOrderId { get; }
            public OrderNo PurchaseOrderOrderNo { get; }
            public IEnumerable<PurchaseOrderLine> LineItems { get; }
            public Dictionary<int, ProductName> ProductOptions { get; set; }
        }

        public class Handler : IRequestHandler<EditLineItemsQuery, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(EditLineItemsQuery request, CancellationToken cancellationToken)
            {
                var purchaseOrder = await _context.PurchaseOrder
                    .Include(o => o.Supplier)
                    .Include(o => o.LineItems)
                    .ThenInclude(li => li.Product)
                    .SingleOrDefaultAsync(o => o.Id == request.PurchaseOrderId);

                if (purchaseOrder == null)
                {
                    throw new PurchaseOrderTrackerException($"Cannot find Purchase Order with id '${request.PurchaseOrderId}'");
                }

                var productOptions = await _context.Product
                    .Where(p => p.SupplierId == purchaseOrder.Supplier.Id)
                    .ToListAsync();

                return new Result(request.PurchaseOrderId, purchaseOrder.OrderNo, purchaseOrder.LineItems,
                    productOptions.ToDictionary(p => p.Id, p => p.Name));
            }
        }
    }
}
