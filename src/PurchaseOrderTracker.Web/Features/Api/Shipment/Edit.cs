using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;

namespace PurchaseOrderTracker.Web.Features.Api.Shipment
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

        public class QueryHandler : IAsyncRequestHandler<Query, QueryResult>
        {
            private readonly PoTrackerDbContext _context;

            public QueryHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<QueryResult> Handle(Query query)
            {
                var shipment = await _context.Shipment.Include(s => s.Status).SingleAsync(s => s.Id == query.Id);
                return Mapper.Map<Domain.Models.ShipmentAggregate.Shipment, QueryResult>(shipment);
            }
        }

        public class Command : IRequest<QueryResult>
        {
            public int? Id { get; set; }

            [Required]
            public string TrackingId { get; set; }

            [Required]
            public string Company { get; set; }

            public DateTime? EstimatedArrivalDate { get; set; }
            public string Comments { get; set; }
            public decimal? ShippingCost { get; set; }
            public string DestinationAddress { get; set; }
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
                var shipment = await _context.Shipment.Include(s => s.Status).SingleAsync(s => s.Id == command.Id);

                Mapper.Map(command, shipment);
                await _context.SaveChangesAsync();

                return Mapper.Map<QueryResult>(shipment);
            }
        }
    }
}