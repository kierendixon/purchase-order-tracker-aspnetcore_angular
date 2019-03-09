using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Web.Infrastructure;
using X.PagedList;
using PagedListExtensions = PurchaseOrderTracker.Web.Infrastructure.PagedListExtensions;

namespace PurchaseOrderTracker.Web.Features.Api.Shipment
{
    public class Inquiry
    {
        public enum QueryType
        {
            All,
            Delayed,
            DelayedMoreThan7Days,
            ScheduledForDeliveryToday
        }

        public class Query : IRequest<Result>
        {
            public int PageSize { get; set; } = 5;
            public int PageNumber { get; set; } = 1;

            [Required]
            public QueryType? QueryType { get; set; }
        }

        public class Result
        {
            public Result(PagedListExtensions.PagedListWebApiObject<ShipmentViewModel> pagedList)
            {
                PagedList = pagedList;
            }

            public PagedListExtensions.PagedListWebApiObject<ShipmentViewModel> PagedList { get; }

            public class ShipmentViewModel
            {
                public ShipmentViewModel(int id, string trackingId, string company, DateTime estimatedArrivalDate,
                    string comments, decimal shippingCost, string status, string destinationAddress, bool isDelayed,
                    bool isDelayedMoreThan7Days, bool isScheduledForDeliveryToday)
                {
                    Id = id;
                    TrackingId = trackingId;
                    Company = company;
                    EstimatedArrivalDate = estimatedArrivalDate;
                    Comments = comments;
                    ShippingCost = shippingCost;
                    Status = status;
                    DestinationAddress = destinationAddress;
                    IsDelayed = isDelayed;
                    IsDelayedMoreThan7Days = isDelayedMoreThan7Days;
                    IsScheduledForDeliveryToday = isScheduledForDeliveryToday;
                }

                public int Id { get; }
                public string TrackingId { get; }
                public string Company { get; }
                public DateTime EstimatedArrivalDate { get; }
                public string Comments { get; }
                public decimal ShippingCost { get; }
                public string Status { get; }
                public string DestinationAddress { get; }

                public bool IsDelayed { get; }
                public bool IsDelayedMoreThan7Days { get; }
                public bool IsScheduledForDeliveryToday { get; }
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IMapper _mapper;
            private readonly IConfigurationProvider _configuration;

            public Handler(PoTrackerDbContext context, IMapper mapper, IConfigurationProvider configuration)
            {
                _context = context;
                _mapper = mapper;
                _configuration = configuration;
            }

            public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
            {
                // Need to call ToList(). which fetches all fields from
                // the database instead of projecting because of a bug in EF Core 2.1
                // https://github.com/aspnet/EntityFrameworkCore/issues/13546
                var shipments = (await _context.Shipment.ToListAsync()).AsQueryable();

                switch (query.QueryType)
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

                var pageOfShipments =
                    new PagedList<Result.ShipmentViewModel>(
                        _mapper.Map<IList<Domain.Models.ShipmentAggregate.Shipment>, IList<Result.ShipmentViewModel>>(shipments.ToList()),
                        query.PageNumber, query.PageSize);

                return new Result(pageOfShipments.ToWebApiObject());
            }
        }
    }
}
