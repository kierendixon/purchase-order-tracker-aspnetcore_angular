using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    public class DeleteProduct
    {
        public class Command : IRequest<Result>
        {
            [Required]
            public int SupplierId { get; set; }

            [Required]
            public int ProductId { get; set; }
        }

        public class Result
        {
            public Result(int supplierId)
            {
                SupplierId = supplierId;
            }

            public int SupplierId { get; }
        }

        public class Handler : IAsyncRequestHandler<Command, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Command command)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var supplier = await _context.Supplier.FindAsync(command.SupplierId);
                var product = await _context.Entry(supplier).Collection(s => s.Products).Query()
                    .Where(p => p.Id == command.ProductId).SingleAsync();

                supplier.RemoveProduct(product);
                await _context.SaveChangesAsync();

                return new Result(command.SupplierId);
            }
        }
    }
}