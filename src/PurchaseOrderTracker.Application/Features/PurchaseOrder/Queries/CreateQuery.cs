using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries.CreateQuery;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries;

public class CreateQuery : IRequest<Result>
{
    public class Result
    {
        public Result(Dictionary<int, SupplierName> supplierOptions)
        {
            SupplierOptions = supplierOptions;
        }

        public Dictionary<int, SupplierName> SupplierOptions { get; }
    }

    public class Handler : IRequestHandler<CreateQuery, Result>
    {
        private readonly PoTrackerDbContext _context;

        public Handler(PoTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _context.Supplier.ToListAsync();
            return new Result(suppliers.ToDictionary(s => s.Id, c => c.Name));
        }
    }
}
