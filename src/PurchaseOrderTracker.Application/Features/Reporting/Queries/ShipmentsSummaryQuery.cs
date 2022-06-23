using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PurchaseOrderTracker.Application.Cache;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.Reporting.Queries.ShipmentsSummaryQuery;

namespace PurchaseOrderTracker.Application.Features.Reporting.Queries;

public class ShipmentsSummaryQuery : IRequest<Result>
{
    // TODO: Purge cache when related data is changed
    [Cache(CacheKeys.ShipmentsSummaryResult)]
    public class Result
    {
        public Result(int totalOpenOrders, int shipmentsSchedForDeliveryToday, int shipmentsDelayed,
            int shipmentsDelayedMoreThan7Days)
        {
            TotalOpenOrders = totalOpenOrders;
            ShipmentsSchedForDeliveryToday = shipmentsSchedForDeliveryToday;
            ShipmentsDelayed = shipmentsDelayed;
            ShipmentsDelayedMoreThan7Days = shipmentsDelayedMoreThan7Days;
        }

        public int TotalOpenOrders { get; }
        public int ShipmentsSchedForDeliveryToday { get; }
        public int ShipmentsDelayed { get; }
        public int ShipmentsDelayedMoreThan7Days { get; }
    }

    public class Handler : IRequestHandler<ShipmentsSummaryQuery, Result>
    {
        private readonly PoTrackerDbContext _context;

        public Handler(PoTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ShipmentsSummaryQuery request, CancellationToken cancellationToken)
        {
            // TODO not performant because all data is retrieved from the database table
            var totalOpenOrders = _context.PurchaseOrder.AsEnumerable().Count(p => p.IsOpen);
            var shipmentsDelayed = _context.Shipment.AsEnumerable().Count(s => s.IsDelayed());
            var shipmentsSchedForDeliveryToday = _context.Shipment.AsEnumerable()
                .Count(s => s.IsScheduledForDeliveryToday());
            var shipmentsDelayedMoreThan7Days = _context.Shipment.AsEnumerable()
                .Count(s => s.IsDelayedMoreThan7Days());

            return new Result(
                totalOpenOrders,
                shipmentsSchedForDeliveryToday,
                shipmentsDelayed,
                shipmentsDelayedMoreThan7Days);
        }
    }
}
