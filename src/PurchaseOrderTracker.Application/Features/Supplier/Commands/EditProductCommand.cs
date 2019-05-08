using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.Supplier.Commands.EditProductCommand;

namespace PurchaseOrderTracker.Application.Features.Supplier.Commands
{
    public class EditProductCommand : IRequest<Result>
    {
        public EditProductCommand(int supplierId, int productId, ProductCode productCode, ProductName productName,
            int? categoryId, decimal price)
        {
            SupplierId = supplierId;
            ProductId = productId;
            ProdCode = productCode;
            ProdName = productName;
            CategoryId = categoryId;
            Price = price;
        }

        public int SupplierId { get; }
        public int ProductId { get; }
        public ProductCode ProdCode { get; }
        public ProductName ProdName { get; }
        public int? CategoryId { get; }
        public decimal Price { get; }

        public class Result
        {
            public Result(int supplierId)
            {
                SupplierId = supplierId;
            }

            public int SupplierId { get; }
        }

        public class Handler : IRequestHandler<EditProductCommand, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(EditProductCommand request, CancellationToken cancellationToken)
            {
                var supplier = await _context.Supplier.FindAsync(request.SupplierId);
                var product = await _context.Entry(supplier).Collection(s => s.Products).Query()
                    .Where(p => p.Id == request.ProductId).SingleAsync();

                product.ProductCode = request.ProdCode;
                product.Name = request.ProdName;
                product.Price = request.Price;
                product.Category = await DetermineNewCategory(_context, supplier, product.Category, request.CategoryId);
                await _context.SaveChangesAsync();

                return new Result(request.SupplierId);
            }

            private async Task<ProductCategory> DetermineNewCategory(
                PoTrackerDbContext context,
                Domain.Models.SupplierAggregate.Supplier supplier,
                ProductCategory modelCategory, int? commandCategoryId)
            {
                if (modelCategory != null && commandCategoryId == null)
                {
                    return null;
                }

                if ((modelCategory == null && commandCategoryId != null) ||
                    (modelCategory != null && modelCategory.Id != commandCategoryId))
                {
                    return await context.Entry(supplier).Collection(s => s.ProductCategories).Query()
                        .Where(c => c.Id == commandCategoryId).SingleAsync();
                }

                return null;
            }
        }
    }
}
