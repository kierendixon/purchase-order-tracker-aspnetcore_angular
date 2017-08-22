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

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    public class Edit
    {
        public class Query : IRequest<QueryResult>
        {
            [Required]
            public int? Id { get; set; }
        }

        public class QueryResult
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

        public class QueryHandler : IAsyncRequestHandler<Query, QueryResult>
        {
            private readonly PoTrackerDbContext _context;

            public QueryHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<QueryResult> Handle(Query query)
            {
                var suppliers = await _context.Supplier.ToListAsync();
                var purchaseOrder = await _context.PurchaseOrder
                    .Include(p => p.Supplier)
                    .Include(p => p.Status)
                    .Include(p => p.Shipment)
                    .ThenInclude(s => s.Status)
                    .SingleAsync(p => p.Id == query.Id);

            var result = Mapper.Map<Domain.Models.PurchaseOrderAggregate.PurchaseOrder, QueryResult>(purchaseOrder);
                result.SupplierOptions = suppliers.ToDictionary(s => s.Id, c => c.Name);
                if (purchaseOrder.CanShipmentBeUpdated)
                {
                    var shipments = (await _context.Shipment.Include(s => s.Status).ToListAsync())
                        .Where(s => s.CanBeAssignedToPurchaseOrder).ToList();
                    result.ShipmentOptions = shipments.ToDictionary(s => s.Id, c => c.TrackingId);
                }

                return result;
            }
        }

        public class Command : IRequest<QueryResult>
        {
            [Required]
            public int? Id { get; set; }

            [Required]
            public int? SupplierId { get; set; }

            [Required]
            public string OrderNo { get; set; }

            public int? ShipmentId { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, QueryResult>
        {
            private readonly PoTrackerDbContext _context;

            public CommandHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<QueryResult> Handle(Command command)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var order = await _context.PurchaseOrder
                    .Include(p => p.Supplier)
                    .Include(p => p.Status)
                    .Include(p => p.Shipment)
                    .ThenInclude(s => s.Status)
                    .SingleAsync(p => p.Id == command.Id);

                order.OrderNo = command.OrderNo;
                await UpdateShipmentIfChanged(command, order);
                await UpdateSupplierIfChanged(command, order);
                await _context.SaveChangesAsync();

                var result = Mapper.Map<Domain.Models.PurchaseOrderAggregate.PurchaseOrder, QueryResult>(order);
                var suppliers = await _context.Supplier.ToListAsync();
                var shipments = (await _context.Shipment.Include(s => s.Status).ToListAsync()).Where(s => s.CanBeAssignedToPurchaseOrder).ToList();
                result.SupplierOptions = suppliers.ToDictionary(s => s.Id, c => c.Name);
                result.ShipmentOptions = shipments.ToDictionary(s => s.Id, c => c.TrackingId);

                return result;
            }

            private async Task UpdateShipmentIfChanged(Command command, Domain.Models.PurchaseOrderAggregate.PurchaseOrder order)
            {
                if (order.Shipment != null && command.ShipmentId == null)
                {
                    if (command.ShipmentId == null)
                    {
                        order.Shipment = null;
                    }
                    else if (order.Shipment.Id != command.ShipmentId)
                    {
                        var shipment = await _context.Shipment.FindAsync(command.ShipmentId);
                        order.Shipment = shipment;
                    }
                }
                else if (order.Shipment == null && command.ShipmentId != null)
                {
                    var shipment = await _context.Shipment.Include(s => s.Status).SingleAsync(s => s.Id == command.ShipmentId);
                    order.Shipment = shipment;
                }
            }

            private async Task UpdateSupplierIfChanged(Command command, Domain.Models.PurchaseOrderAggregate.PurchaseOrder order)
            {
                if (order.Supplier.Id != command.SupplierId)
                {
                    _context.Entry(order).Collection(o => o.LineItems).Load();
                    var supplier = await _context.Supplier.FindAsync(command.SupplierId);
                    order.ChangeSupplier(supplier);
                }
            }
        }
    }
}