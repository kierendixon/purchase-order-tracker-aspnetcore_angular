using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Web.Infrastructure;

namespace PurchaseOrderTracker.Web.Features.Api.Shipment
{
    public class Create
    {
        public class Command : IRequest<Result>
        {
            [Required]
            public string TrackingId { get; set; }

            [Required]
            public string Company { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime? EstimatedArrivalDate { get; set; }

            public string Comments { get; set; }
            public decimal? ShippingCost { get; set; }
            public string DestinationAddress { get; set; }
        }

        public class Result
        {
            public Result(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IMapper _mapper;

            public Handler(PoTrackerDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                    var shipment = _mapper.Map<Domain.Models.ShipmentAggregate.Shipment>(command);
                    _context.Shipment.Add(shipment);
                    await _context.SaveChangesAsync();
                    return new Result(shipment.Id);
                }
                catch (DbUpdateException ex)
                {
                    if (ex.IsDuplicateKeyError())
                        throw new PurchaseOrderTrackerException("A duplicate shipment already exists");

                    throw;
                }
            }
        }
    }
}