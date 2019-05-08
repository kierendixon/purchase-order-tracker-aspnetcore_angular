using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands.DeleteLineItemCommand;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands
{
    public class DeleteLineItemCommand : IRequest<Result>
    {
        public DeleteLineItemCommand(int purchaseOrderId, int lineItemId)
        {
            PurchaseOrderId = purchaseOrderId;
            LineItemId = lineItemId;
        }

        public int PurchaseOrderId { get; set; }
        public int LineItemId { get; set; }

        public class Result
        {
            public Result(int purchaseOrderId)
            {
                PurchaseOrderId = purchaseOrderId;
            }

            public int PurchaseOrderId { get; }
        }

        public class Handler : IRequestHandler<DeleteLineItemCommand, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(DeleteLineItemCommand request, CancellationToken cancellationToken)
            {
                var purchaseOrder = await _context.PurchaseOrder.Include(o => o.LineItems).SingleAsync(o => o.Id == request.PurchaseOrderId);
                var lineItem = purchaseOrder.LineItems.Single(li => li.Id == request.LineItemId);

                purchaseOrder.RemoveLineItem(lineItem);
                await _context.SaveChangesAsync();

                return new Result(request.PurchaseOrderId);
            }
        }
    }
}
