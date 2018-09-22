using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    public class EditProduct
    {
        public class Command : IRequest<Result>
        {
            [Required]
            public int SupplierId { get; set; }

            [Required]
            public int ProductId { get; set; }

            [Required]
            public string ProdCode { get; set; }

            [Required]
            [StringLength(150, MinimumLength = 3)]
            public string Name { get; set; }

            public int? CategoryId { get; set; }

            [Required]
            public decimal? Price { get; set; }
        }

        public class Result
        {
            public Result(int supplierId)
            {
                SupplierId = supplierId;
            }

            public int SupplierId { get; }
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
                var supplier = await _context.Supplier.FindAsync(command.SupplierId);
                var product = await _context.Entry(supplier).Collection(s => s.Products).Query()
                    .Where(p => p.Id == command.ProductId).SingleAsync();

                product.ProdCode = command.ProdCode;
                product.Name = command.Name;
                product.Price = command.Price.Value;
                product.Category = await DetermineNewCategory(_context, supplier, product.Category, command.CategoryId);
                await _context.SaveChangesAsync();

                return new Result(command.SupplierId);
            }

            private async Task<ProductCategory> DetermineNewCategory(PoTrackerDbContext context,
                Domain.Models.SupplierAggregate.Supplier supplier,
                ProductCategory modelCategory, int? commandCategoryId)
            {
                if (modelCategory != null && commandCategoryId == null)
                    return null;
                if (modelCategory == null && commandCategoryId != null ||
                    modelCategory != null && modelCategory.Id != commandCategoryId)
                    return await context.Entry(supplier).Collection(s => s.ProductCategories).Query()
                        .Where(c => c.Id == commandCategoryId).SingleAsync();
                return null;
            }
        }
    }
}