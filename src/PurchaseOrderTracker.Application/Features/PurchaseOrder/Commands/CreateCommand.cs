using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands.CreateCommand;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands;

public class CreateCommand : IRequest<Result>
{
    public CreateCommand(OrderNo orderNo, int supplierId)
    {
        OrderNo = orderNo;
        SupplierId = supplierId;
    }

    public OrderNo OrderNo { get; }
    public int SupplierId { get; }

    // TODO implement ToString in other commands/queries so that MediatrLoggingBehaviour prints useful log messages
    public override string ToString()
    {
        return $"{nameof(OrderNo)}: {OrderNo}, {nameof(SupplierId)}: {SupplierId}";
    }

    // TODO convert to record type?
    public class Result
    {
        public Result(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; }
    }

    public class Handler : IRequestHandler<CreateCommand, Result>
    {
        private readonly PoTrackerDbContext _context;

        public Handler(PoTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _context.Supplier.FindAsync(request.SupplierId);
            var purchaseOrder = new Domain.Models.PurchaseOrderAggregate.PurchaseOrder(request.OrderNo, supplier);
            _context.PurchaseOrder.Add(purchaseOrder);

            await _context.SaveChangesAsync();

            return new Result(purchaseOrder.Id);
        }
    }
}
