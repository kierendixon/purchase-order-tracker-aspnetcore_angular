using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Web.Features.Notifications;
using PurchaseOrderTracker.Web.Infrastructure;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    public class Create
    {
        public class Command : IRequest<CommandResult>
        {
            [Required]
            [StringLength(150, MinimumLength = 3)]
            public string Name { get; set; }
        }

        public class CommandResult
        {
            public CommandResult(int supplierId)
            {
                SupplierId = supplierId;
            }

            public int SupplierId { get; }
        }

        public class Notification : INotification, ICreateNotification
        {
            public Notification(int supplierId)
            {
                _supplierId = supplierId;
            }

            private readonly int _supplierId;

            public int GetEntityId() => _supplierId;

            public Type GetEntityType() => typeof(Domain.Models.SupplierAggregate.Supplier);
        }

        public class Handler : IRequestHandler<Command, CommandResult>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;

            public Handler(PoTrackerDbContext context, IMapper mapper, IMediator mediator)
            {
                _context = context;
                _mapper = mapper;
                _mediator = mediator;
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                    var supplier = _mapper.Map<Domain.Models.SupplierAggregate.Supplier>(command);
                    _context.Supplier.Add(supplier);
                    await _context.SaveChangesAsync();

                    await _mediator.Publish(new Notification(supplier.Id));

                    return new CommandResult(supplier.Id);
                }
                catch (DbUpdateException ex)
                {
                    if (ex.IsDuplicateKeyError())
                        throw new PurchaseOrderTrackerException($"A supplier with the name {command.Name} already exists");

                    throw;
                }
            }
        }
    }
}
