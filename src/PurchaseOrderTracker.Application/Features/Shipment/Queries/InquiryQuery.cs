using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PurchaseOrderTracker.Persistence;
using X.PagedList;
using static PurchaseOrderTracker.Application.Features.Shipment.Queries.InquiryQuery;

namespace PurchaseOrderTracker.Application.Features.Shipment.Queries
{
    public class InquiryQuery : IRequest<Result>
    {
        public InquiryQuery(int? pageSize, int? pageNumber, QueryType inquiryQueryType)
        {
            PageSize = pageSize ?? 5;
            PageNumber = pageNumber ?? 1;
            InquiryQueryType = inquiryQueryType;
        }

        public enum QueryType
        {
            All,
            Delayed,
            DelayedMoreThan7Days,
            ScheduledForDeliveryToday
        }

        public int PageSize { get; }
        public int PageNumber { get; }
        public QueryType InquiryQueryType { get; }

        public class Result
        {
            public Result(PagedList<Domain.Models.ShipmentAggregate.Shipment> pagedList)
            {
                PagedList = pagedList;
            }

            public PagedList<Domain.Models.ShipmentAggregate.Shipment> PagedList { get; }
        }

        public class Handler : IRequestHandler<InquiryQuery, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(InquiryQuery request, CancellationToken cancellationToken)
            {
                // Need to call ToList(). which fetches all fields from
                // the database instead of projecting because of a bug in EF Core 2.1
                // https://github.com/aspnet/EntityFrameworkCore/issues/13546
                var shipments = (await _context.Shipment.ToListAsync()).AsQueryable();

                switch (request.InquiryQueryType)
                {
                    case QueryType.All:
                        // do nothing
                        break;
                    case QueryType.Delayed:
                        shipments = shipments.Where(s => s.IsDelayed());
                        break;
                    case QueryType.DelayedMoreThan7Days:
                        shipments = shipments.Where(s => s.IsDelayedMoreThan7Days());
                        break;
                    case QueryType.ScheduledForDeliveryToday:
                        shipments = shipments.Where(s => s.IsScheduledForDeliveryToday());
                        break;
                }

                // Can't project with AutoMapper due to bugs. See notes in MappingProfile.cs
                // var pageOfShipments = await shipmentsList.AsQueryable()
                //    .ProjectToPagedList<Result.ShipmentViewModel>(_configuration, query.PageNumber, query.PageSize);
                var pageOfShipments = new PagedList<Domain.Models.ShipmentAggregate.Shipment>(
                    shipments, request.PageNumber, request.PageSize);

                return new Result(pageOfShipments);
            }
        }
    }
}
