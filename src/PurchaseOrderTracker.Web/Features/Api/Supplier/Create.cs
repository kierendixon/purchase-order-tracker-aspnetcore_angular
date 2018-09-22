using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;
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

        public class Handler : IRequestHandler<Command, CommandResult>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IMapper _mapper;

            public Handler(PoTrackerDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                    var supplier = _mapper.Map<Domain.Models.SupplierAggregate.Supplier>(command);
                    _context.Supplier.Add(supplier);
                    await _context.SaveChangesAsync();

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