using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    public class EditProductCategory
    {
        public class Command : IRequest
        {
            [Required]
            public int? SupplierId { get; set; }

            [Required]
            public int? CategoryId { get; set; }

            [Required]
            [StringLength(150, MinimumLength = 3)]
            public string Name { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task Handle(Command command)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var supplier = await _context.Supplier.FindAsync(command.SupplierId);
                var category = await _context.Entry(supplier).Collection(s => s.ProductCategories).Query()
                    .Where(c => c.Id == command.CategoryId).SingleAsync();

                category.Name = command.Name;
                await _context.SaveChangesAsync();
            }
        }
    }
}