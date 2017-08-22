using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
                    string comments, decimal shippingCost, string currentState, string destinationAddress, bool isDelayed,
                    bool isDelayedMoreThan7Days, bool isScheduledForDeliveryToday)
                {
                    Id = id;
                    TrackingId = trackingId;
                    Company = company;
                    EstimatedArrivalDate = estimatedArrivalDate;
                    Comments = comments;
                    ShippingCost = shippingCost;
                    CurrentState = currentState;
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
                public string CurrentState { get; }
                public string DestinationAddress { get; }

                public bool IsDelayed { get; }
                public bool IsDelayedMoreThan7Days { get; }
                public bool IsScheduledForDeliveryToday { get; }
            }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Query query)
            {
                var shipments = _context.Shipment.Include(s => s.Status).AsQueryable();
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

                // LINQ can't project computed values, and the DelegateCompile package to decompile computed values doesn't suport .NET Core 1.1
                // Perform paging manually instead of using ProjectToPagedList()
//                var pageOfShipments = await shipments.Skip(query.PageSize * (query.PageNumber - 1)).Take(query.PageSize)
//                    .ToListAsync();
                var pageOfShipments = await shipments.ToListAsync();

                var paginatedShipments =
                    new PagedList<Result.ShipmentViewModel>(
                        Mapper.Map<IList<Domain.Models.ShipmentAggregate.Shipment>, IList<Result.ShipmentViewModel>>(pageOfShipments),
                        query.PageNumber, query.PageSize);

                return new Result(paginatedShipments.ToWebApiObject());
            }
        }
    }
}