using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
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
                    string status)
                {
                    Id = id;
                    OrderNo = orderNo;
                    CreatedDate = createdDate;
                    SupplierName = supplierName;
                    Status = status;
                }

                public int Id { get; }
                public string OrderNo { get; }
                public DateTime CreatedDate { get; }
                public string SupplierName { get; }
                public string Status { get; }
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
                // Need to call ToList() which fetches all fields from
                // the database instead of projecting because of a bug in EF Core 2.1
                // https://github.com/aspnet/EntityFrameworkCore/issues/13546
                var orders = (await _context.PurchaseOrder
                    .Include(o => o.Supplier)
                    .Include(o => o.Shipment)
                    .ToListAsync())
                    .AsQueryable();

                switch (query.QueryType)
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

                var paginatedOrders =
                    new PagedList<Result.PurchaseOrderViewModel>(
                        _mapper.Map<IQueryable<Domain.Models.PurchaseOrderAggregate.PurchaseOrder>, IList<Result.PurchaseOrderViewModel>>(orders),
                        query.PageNumber, query.PageSize);
               
                return new Result(paginatedOrders.ToWebApiObject());
            }
        }
    }
}