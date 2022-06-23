using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.Supplier.Commands.DeleteProductCommand;

namespace PurchaseOrderTracker.Application.Features.Supplier.Commands;

public class DeleteProductCommand : IRequest<Result>
{
    public DeleteProductCommand(int supplierId, int productId)
    {
        SupplierId = supplierId;
        ProductId = productId;
    }

    public int SupplierId { get; }
    public int ProductId { get; }

    public class Result
    {
        public Result(int supplierId)
        {
            SupplierId = supplierId;
        }

        public int SupplierId { get; }
    }

    public class Handler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly PoTrackerDbContext _context;

        public Handler(PoTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _context.Supplier.FindAsync(request.SupplierId);
            var product = await _context.Entry(supplier).Collection(s => s.Products).Query()
                .Where(p => p.Id == request.ProductId).SingleAsync();

            supplier.RemoveProduct(product);
            await _context.SaveChangesAsync();

            return new Result(request.SupplierId);
        }
    }
}
