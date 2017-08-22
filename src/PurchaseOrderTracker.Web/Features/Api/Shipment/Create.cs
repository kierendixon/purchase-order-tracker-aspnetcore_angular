using System;
using System.ComponentModel.DataAnnotations;
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

        public class Handler : IAsyncRequestHandler<Command, Result>
        {
            private readonly PoTrackerDbContext _context;

            public Handler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Command command)
            {
                try
                {
                    var shipment = Mapper.Map<Domain.Models.ShipmentAggregate.Shipment>(command);
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