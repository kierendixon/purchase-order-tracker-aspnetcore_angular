using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Web.Infrastructure;
using X.PagedList;
using PagedListExtensions = PurchaseOrderTracker.Web.Infrastructure.PagedListExtensions;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    public class Inquiry
    {
        public enum QueryType
        {
            Open,
            All,
            ScheduledForDeliveryToday,
            Delayed,
            DelayedMoreThan7Days
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
            public Result(PagedListExtensions.PagedListWebApiObject<PurchaseOrderViewModel> pagedList)
            {
                PagedList = pagedList;
            }

            public PagedListExtensions.PagedListWebApiObject<PurchaseOrderViewModel> PagedList { get; }

            public class PurchaseOrderViewModel
            {
                public PurchaseOrderViewModel(int id, string orderNo, DateTime createdDate, string supplierName,
                    string currentState)
                {
                    Id = id;
                    OrderNo = orderNo;
                    CreatedDate = createdDate;
                    SupplierName = supplierName;
                    CurrentState = currentState;
                }

                public int Id { get; }
                public string OrderNo { get; }
                public DateTime CreatedDate { get; }
                public string SupplierName { get; }
                public string CurrentState { get; }
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
                // Due to similar bugs described in the /Home/Index.cs source file we retrieve all data from 
                // the database and then filter in memory
                var allOrders = await _context.PurchaseOrder
                    .Include(o => o.Supplier)
                    .Include(o => o.Status)
                    .Include(o => o.Shipment)
                        .ThenInclude(s => s.Status)
                    .ToListAsync();

                IQueryable<Domain.Models.PurchaseOrderAggregate.PurchaseOrder> orders = null;
                switch (query.QueryType)
                {
                    case QueryType.All:
                        orders = allOrders.AsQueryable();
                        break;
                    case QueryType.Open:
                        orders = allOrders.Where(o => o.IsOpen).AsQueryable();
                        break;
                    case QueryType.ScheduledForDeliveryToday:
                        orders = allOrders.Where(o => o.Shipment != null && o.Shipment.IsScheduledForDeliveryToday()).AsQueryable();
                        break;
                    case QueryType.Delayed:
                        orders = allOrders.Where(o => o.Shipment != null && o.Shipment.IsDelayed()).AsQueryable();
                        break;
                    case QueryType.DelayedMoreThan7Days:
                        orders = allOrders.Where(o => o.Shipment != null && o.Shipment.IsDelayedMoreThan7Days()).AsQueryable();
                        break;
                }

                // var paginatedOrders = await orders.ProjectToPagedList<Result.PurchaseOrderViewModel>(query.PageNumber, query.PageSize);
                var paginatedOrders =
                    new PagedList<Result.PurchaseOrderViewModel>(
                        Mapper.Map<IQueryable<Domain.Models.PurchaseOrderAggregate.PurchaseOrder>, IList<Result.PurchaseOrderViewModel>>(orders),
                        query.PageNumber, query.PageSize);

                if (paginatedOrders.Count == 0)
                    throw new PurchaseOrderTrackerException("No Purchase Orders found");

                return new Result(paginatedOrders.ToWebApiObject());
            }
        }
    }
}