using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Persistence;
using X.PagedList;
using static PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries.InquiryQuery;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries
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
            Open,
            All,
            ScheduledForDeliveryToday,
            Delayed,
            DelayedMoreThan7Days
        }

        public int PageSize { get; }
        public int PageNumber { get; }
        public QueryType InquiryQueryType { get; }

        public class Result
        {
            public Result(PagedList<Domain.Models.PurchaseOrderAggregate.PurchaseOrder> pagedList)
            {
                PagedList = pagedList;
            }

            public PagedList<Domain.Models.PurchaseOrderAggregate.PurchaseOrder> PagedList { get; }
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
                // Need to call ToList() which fetches all fields from
                // the database instead of projecting because of a bug in EF Core 2.1
                // https://github.com/aspnet/EntityFrameworkCore/issues/13546
                var orders = (await _context.PurchaseOrder
                    .Include(o => o.Supplier)
                    .Include(o => o.Shipment)
                    .ToListAsync())
                    .AsQueryable();

                switch (request.InquiryQueryType)
                {
                    case QueryType.All:
                        orders = orders.AsQueryable();
                        break;
                    case QueryType.Open:
                        orders = orders.Where(o => o.IsOpen).AsQueryable();
                        break;
                    case QueryType.ScheduledForDeliveryToday:
                        orders = orders.Where(o => o.Shipment != null && o.Shipment.IsScheduledForDeliveryToday()).AsQueryable();
                        break;
                    case QueryType.Delayed:
                        orders = orders.Where(o => o.Shipment != null && o.Shipment.IsDelayed()).AsQueryable();
                        break;
                    case QueryType.DelayedMoreThan7Days:
                        orders = orders.Where(o => o.Shipment != null && o.Shipment.IsDelayedMoreThan7Days()).AsQueryable();
                        break;
                }

                // Can't project with AutoMapper due to bugs. See notes in MappingProfile.cs
                // var pageOfOrders = await orders.ToList().AsQueryable().
                //    ProjectToPagedList<Result.PurchaseOrderViewModel>(_configuration, query.PageNumber, query.PageSize);
                var paginatedOrders = new PagedList<Domain.Models.PurchaseOrderAggregate.PurchaseOrder>(
                    orders, request.PageNumber, request.PageSize);

                return new Result(paginatedOrders);
            }
        }
    }
}
