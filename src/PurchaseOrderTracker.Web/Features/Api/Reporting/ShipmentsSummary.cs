using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;

namespace PurchaseOrderTracker.Web.Features.Api.Reporting
{

    public class ShipmentsSummary
    {
        public class Query : IRequest<Result>
        {
        }

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

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
            {
                // Need to call ToAsyncEnumerable().Count(filter) which fetches all fields from
                // the database instead of CountAsync(filter) because of a bug in EF Core 2.1
                // https://github.com/aspnet/EntityFrameworkCore/issues/13546

                var totalOpenOrders = await _context.PurchaseOrder.ToAsyncEnumerable()
                    .Count(p => p.IsOpen);
                var shipmentsDelayed = await _context.Shipment.ToAsyncEnumerable()
                    .Count(s => s.IsDelayed());
                var shipmentsSchedForDeliveryToday =
                    await _context.Shipment.CountAsync(s => s.IsScheduledForDeliveryToday());
                var shipmentsDelayedMoreThan7Days = await _context.Shipment.ToAsyncEnumerable()
                    .Count(s => s.IsDelayedMoreThan7Days());

                return new Result(totalOpenOrders, shipmentsSchedForDeliveryToday, shipmentsDelayed,
                    shipmentsDelayedMoreThan7Days);
            }
        }
    }
}