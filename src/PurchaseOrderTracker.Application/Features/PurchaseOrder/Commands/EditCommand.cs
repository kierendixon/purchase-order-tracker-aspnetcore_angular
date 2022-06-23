using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries.EditQuery;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder.Commands;

public class EditCommand : IRequest<Result>
{
    public EditCommand(int purchaseOrderId, int supplierId, OrderNo orderNo, int? shipmentId)
    {
        PurchaseOrderId = purchaseOrderId;
        SupplierId = supplierId;
        OrderNo = orderNo ?? throw new ArgumentNullException(nameof(orderNo));
        ShipmentId = shipmentId;
    }

    public int PurchaseOrderId { get; set; }
    public int SupplierId { get; set; }
    public OrderNo OrderNo { get; set; }
    public int? ShipmentId { get; set; }

    public class Handler : IRequestHandler<EditCommand, Result>
    {
        private readonly PoTrackerDbContext _context;
        private readonly IMapper _mapper;

        public Handler(PoTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(EditCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.PurchaseOrder
                .Include(p => p.Supplier)
                .Include(p => p.Status)
                .Include(p => p.Shipment)
                .ThenInclude(s => s.Status)
                .SingleAsync(p => p.Id == request.PurchaseOrderId);

            order.OrderNo = request.OrderNo;
            await UpdateShipmentIfChanged(request, order);
            await UpdateSupplierIfChanged(request, order);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<Domain.Models.PurchaseOrderAggregate.PurchaseOrder, Result>(order);
            var suppliers = await _context.Supplier.ToListAsync();
            var shipments = (await _context.Shipment.ToListAsync()).Where(s => s.CanBeAssignedToPurchaseOrder).ToList();
            result.SupplierOptions = suppliers.ToDictionary(s => s.Id, c => c.Name.Value);
            result.ShipmentOptions = shipments.ToDictionary(s => s.Id, c => c.TrackingId);

            return result;
        }

        private async Task UpdateShipmentIfChanged(EditCommand command, Domain.Models.PurchaseOrderAggregate.PurchaseOrder order)
        {
            if (order.Shipment != null && command.ShipmentId == null)
            {
                order.Shipment = null;
            }
            else if (order.Shipment == null && command.ShipmentId != null)
            {
                var shipment = await _context.Shipment.SingleAsync(s => s.Id == command.ShipmentId);
                order.Shipment = shipment;
            }
        }

        private async Task UpdateSupplierIfChanged(EditCommand command, Domain.Models.PurchaseOrderAggregate.PurchaseOrder order)
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
