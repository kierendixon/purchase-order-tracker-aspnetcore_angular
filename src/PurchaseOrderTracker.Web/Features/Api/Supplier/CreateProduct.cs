using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Web.Infrastructure;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    public class CreateProduct
    {
        public class Query : IRequest<Command>
        {
            [Required]
            public int SupplierId { get; set; }
        }

        public class Command : IRequest<CommandResult>
        {
            public Command()
            {
            }

            public Command(int supplierId, Dictionary<int, string> categoryOptions)
            {
                SupplierId = supplierId;
                CategoryOptions = categoryOptions;
            }

            [Required]
            public int SupplierId { get; set; }

            [Required]
            public string ProdCode { get; set; }

            [Required]
            [StringLength(150, MinimumLength = 3)]
            public string Name { get; set; }

            [Required]
            public int CategoryId { get; set; }

            public double? Price { get; set; }

            [BindNever]
            public Dictionary<int, string> CategoryOptions { get; }
        }

        public class CommandResult
        {
            public CommandResult(int supplierId)
            {
                SupplierId = supplierId;
            }

            public int SupplierId { get; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Command>
        {
            private readonly PoTrackerDbContext _context;

            public QueryHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Command> Handle(Query query)
            {
                var supplier = await _context.Supplier.Include(s => s.ProductCategories)
                    .SingleAsync(s => s.Id == query.SupplierId);
                return new Command(query.SupplierId, supplier.ProductCategories.ToDictionary(c => c.Id, c => c.Name));
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, CommandResult>
        {
            private readonly PoTrackerDbContext _context;

            public CommandHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<CommandResult> Handle(Command command)
            {
                try
                {
                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                    var supplier = await _context.Supplier.FindAsync(command.SupplierId);
                    var category = _context.ProductCategory.Single(c => c.Id == command.CategoryId);
                    var product = Mapper.Map<Product>(command);
                    product.Category = category;

                    supplier.AddProduct(product);
                    await _context.SaveChangesAsync();

                    return new CommandResult(command.SupplierId);
                }
                catch (DbUpdateException ex)
                {
                    if (ex.IsDuplicateKeyError())
                        throw new PurchaseOrderTrackerException("A duplicate product already exists");

                    throw;
                }
            }
        }
    }
}