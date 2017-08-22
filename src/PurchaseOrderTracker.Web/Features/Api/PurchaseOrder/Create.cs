using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    public class Create
    {
        public class Query : IRequest<QueryResult>
        {
        }

        public class QueryResult
        {
            public QueryResult(Dictionary<int, string> supplierOptions)
            {
                SupplierOptions = supplierOptions;
            }

            public Dictionary<int, string> SupplierOptions { get; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, QueryResult>
        {
            private readonly PoTrackerDbContext _context;

            public QueryHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<QueryResult> Handle(Query query)
            {
                var supplier = await _context.Supplier.ToListAsync();
                return new QueryResult(supplier.ToDictionary(s => s.Id, c => c.Name));
            }
        }

        public class Command : IRequest<CommandResult>
        {
            [Required]
            [StringLength(150, MinimumLength = 3)]
            public string OrderNo { get; set; }

            [Required]
            public int? SupplierId { get; set; }
        }

        public class CommandResult
        {
            public CommandResult(int orderId)
            {
                OrderId = orderId;
            }

            public int OrderId { get; }
        }

        public class CreateHandler : IAsyncRequestHandler<Command, CommandResult>
        {
            private readonly PoTrackerDbContext _context;

            public CreateHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<CommandResult> Handle(Command command)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var supplier = await _context.Supplier.FindAsync(command.SupplierId);

                var purchaseOrder = new Domain.Models.PurchaseOrderAggregate.PurchaseOrder(command.OrderNo, supplier);
                _context.PurchaseOrder.Add(purchaseOrder);
                await _context.SaveChangesAsync();

                return new CommandResult(purchaseOrder.Id);
            }
        }
    }
}