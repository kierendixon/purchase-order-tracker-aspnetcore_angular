using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.Shipment.Queries.EditQuery;

namespace PurchaseOrderTracker.Application.Features.Shipment.Queries;

public class EditQuery : IRequest<Result>
{
    public EditQuery(int shipmentId)
    {
        ShipmentId = shipmentId;
    }

    public int ShipmentId { get; }

    public class Result
    {
        public int Id { get; private set; }
        public string TrackingId { get; private set; }
        public string Company { get; private set; }
        public DateTime EstimatedArrivalDate { get; private set; }
        public string Comments { get; private set; }
        public decimal ShippingCost { get; private set; }
        public string CurrentState { get; private set; }
        public string DestinationAddress { get; private set; }

        public bool IsDelivered { get; private set; }
        public bool CanTransitionToAwaitingShipping { get; private set; }
        public bool CanTransitionToShipped { get; private set; }
        public bool CanTransitionToDelivered { get; private set; }
    }

    public class Handler : IRequestHandler<EditQuery, Result>
    {
        private readonly PoTrackerDbContext _context;
        private readonly IMapper _mapper;

        public Handler(PoTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(EditQuery request, CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipment.SingleAsync(s => s.Id == request.ShipmentId);
            return _mapper.Map<Domain.Models.ShipmentAggregate.Shipment, Result>(shipment);
        }
    }
}
