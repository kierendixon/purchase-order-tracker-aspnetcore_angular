using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.Shipment.Queries.EditQuery;

namespace PurchaseOrderTracker.Application.Features.Shipment.Commands;

// TODO don't return query's result object
public class EditCommand : IRequest<Result>
{
    public EditCommand(int shipmentId, string trackingId, string company, DateTime? estimatedArrivalDate,
        string comments, decimal? shippingCost, string destinationAddress)
    {
        ShipmentId = shipmentId;
        TrackingId = trackingId ?? throw new ArgumentNullException(nameof(trackingId));
        Company = company ?? throw new ArgumentNullException(nameof(trackingId));
        EstimatedArrivalDate = estimatedArrivalDate;
        Comments = comments;
        ShippingCost = shippingCost;
        DestinationAddress = destinationAddress;
    }

    public int ShipmentId { get; }
    public string TrackingId { get; }
    public string Company { get; }
    public DateTime? EstimatedArrivalDate { get; }
    public string Comments { get; }
    public decimal? ShippingCost { get; }
    public string DestinationAddress { get; }

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
            var shipment = await _context.Shipment.SingleAsync(s => s.Id == request.ShipmentId);

            _mapper.Map(request, shipment);
            await _context.SaveChangesAsync();

            return _mapper.Map<Result>(shipment);
        }
    }
}
