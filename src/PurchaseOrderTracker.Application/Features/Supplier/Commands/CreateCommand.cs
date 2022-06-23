using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Application.Notifications;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;

namespace PurchaseOrderTracker.Application.Features.Supplier.Commands;

public class CreateCommand : IRequest<CreateCommand.Result>
{
    public CreateCommand(SupplierName name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public SupplierName Name { get; }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}";
    }

    public class Result
    {
        public Result(int supplierId)
        {
            SupplierId = supplierId;
        }

        public int SupplierId { get; }
    }

    public class Notification : ICreateNotification
    {
        private readonly int _supplierId;

        public Notification(int supplierId)
        {
            _supplierId = supplierId;
        }

        public int GetEntityId()
        {
            return _supplierId;
        }

        public Type GetEntityType()
        {
            return typeof(Domain.Models.SupplierAggregate.Supplier);
        }
    }

    public class Handler : IRequestHandler<CreateCommand, Result>
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

        public async Task<Result> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var supplier = _mapper.Map<Domain.Models.SupplierAggregate.Supplier>(request);
                _context.Supplier.Add(supplier);
                await _context.SaveChangesAsync();

                await _mediator.Publish(new Notification(supplier.Id));

                return new Result(supplier.Id);
            }
            catch (DbUpdateException ex)
            {
                if (ex.IsDuplicateKeyError())
                {
                    throw new PurchaseOrderTrackerException($"A supplier with the name {request.Name} already exists");
                }

                throw;
            }
        }
    }
}
