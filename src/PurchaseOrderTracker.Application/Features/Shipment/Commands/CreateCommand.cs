using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.Shipment.Commands.CreateCommand;

namespace PurchaseOrderTracker.Application.Features.Shipment.Commands
{
    public class CreateCommand : IRequest<Result>
    {
        public CreateCommand(string trackingId, string company, DateTime estimatedArrivalDate,
            string comments, decimal? shippingCost, string destinationAddress)
        {
            TrackingId = trackingId ?? throw new ArgumentNullException(nameof(trackingId));
            Company = company ?? throw new ArgumentNullException(nameof(company));
            EstimatedArrivalDate = estimatedArrivalDate;
            Comments = comments;
            ShippingCost = shippingCost;
            DestinationAddress = destinationAddress;
        }

        public string TrackingId { get; }
        public string Company { get; }
        public DateTime? EstimatedArrivalDate { get; }
        public string Comments { get; }
        public decimal? ShippingCost { get; }
        public string DestinationAddress { get; }

        public class Result
        {
            public Result(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Handler : IRequestHandler<CreateCommand, Result>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IMapper _mapper;

            public Handler(PoTrackerDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result> Handle(CreateCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var shipment = _mapper.Map<Domain.Models.ShipmentAggregate.Shipment>(request);
                    _context.Shipment.Add(shipment);
                    await _context.SaveChangesAsync();
                    return new Result(shipment.Id);
                }
                catch (DbUpdateException ex)
                {
                    if (ex.IsDuplicateKeyError())
                    {
                        // TODO: throw a new type of exception which is handled in the request pipeline
                        // with a generic "duplicate record" error message
                        throw new PurchaseOrderTrackerException("A duplicate shipment already exists");
                    }

                    throw;
                }
            }
        }
    }
}
