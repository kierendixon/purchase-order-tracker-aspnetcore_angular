using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries.EditQuery;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries
{
    public class EditQuery : IRequest<Result>
    {
        public EditQuery(int purchaseOrderId)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public int PurchaseOrderId { get; }

        public class Result
        {
            public int Id { get; private set; }
            public DateTime CreatedDate { get; private set; }
            public int SupplierId { get; private set; }
            public int? ShipmentId { get; private set; }
            public string ShipmentTrackingId { get; private set; }
            public string CurrentState { get; private set; }
            public string OrderNo { get; private set; }

            public bool IsApprovedOrLaterStatus { get; private set; }
            public bool IsDelivered { get; private set; }
            public bool IsCancelled { get; private set; }
            public bool IsApproved { get; private set; }
            public bool CanTransitionToPendingApproval { get; private set; }
            public bool CanTransitionToApproved { get; private set; }
            public bool CanTransitionToCancelled { get; private set; }
            public bool CanShipmentBeUpdated { get; private set; }

            public Dictionary<int, string> SupplierOptions { get; set; }
            public Dictionary<int, string> ShipmentOptions { get; set; }
        }

        public class QueryHandler : IRequestHandler<EditQuery, Result>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(PoTrackerDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result> Handle(EditQuery request, CancellationToken cancellationToken)
            {
                var suppliers = await _context.Supplier.ToListAsync();

                // TODO: this should return 404 instead of causing a 500
                var purchaseOrder = await _context.PurchaseOrder
                    .Include(p => p.Supplier)
                    .Include(p => p.Status)
                    .Include(p => p.Shipment)
                    .ThenInclude(s => s.Status)
                    .SingleAsync(p => p.Id == request.PurchaseOrderId);

                var result = _mapper.Map<Domain.Models.PurchaseOrderAggregate.PurchaseOrder, Result>(purchaseOrder);
                result.SupplierOptions = suppliers.ToDictionary(s => s.Id, c => c.Name.Value);
                if (purchaseOrder.CanShipmentBeUpdated)
                {
                    var shipments = (await _context.Shipment.ToListAsync())
                        .Where(s => s.CanBeAssignedToPurchaseOrder).ToList();
                    result.ShipmentOptions = shipments.ToDictionary(s => s.Id, c => c.TrackingId);
                }

                return result;
            }
        }
    }
}
