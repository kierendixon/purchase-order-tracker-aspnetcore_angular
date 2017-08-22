using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    public class DeleteLineItem
    {
        public class Command : IRequest<Result>
        {
            [Required]
            [FromRoute(Name = "Id")]
            public int? PurchaseOrderId { get; set; }

            [Required]
            public int? LineItemId { get; set; }
        }

        public class Result
        {
            public Result(int purchaseOrderId)
            {
                PurchaseOrderId = purchaseOrderId;
            }

            public int PurchaseOrderId { get; }
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
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var purchaseOrder = await _context.PurchaseOrder.Include(o => o.LineItems).SingleAsync(o => o.Id == command.PurchaseOrderId);
                var lineItem = purchaseOrder.LineItems.Single(li => li.Id == command.LineItemId);

                purchaseOrder.RemoveLineItem(lineItem);
                await _context.SaveChangesAsync();

                return new Result(command.PurchaseOrderId.Value);
            }
        }
    }
}