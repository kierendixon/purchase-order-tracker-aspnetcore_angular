using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using PurchaseOrderTracker.Persistence;
using static PurchaseOrderTracker.Application.Features.Supplier.Queries.EditQuery;

namespace PurchaseOrderTracker.Application.Features.Supplier.Commands
{
    public class EditCommand : IRequest<Result>
    {
        public EditCommand(int supplierId, SupplierName name)
        {
            SupplierId = supplierId;
            Name = name;
        }

        public int SupplierId { get; set; }
        public SupplierName Name { get; set; }

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
                var supplier = await _context.Supplier.FindAsync(request.SupplierId);

                _mapper.Map(request, supplier);
                await _context.SaveChangesAsync();

                return new Result(_mapper.Map<Result.SupplierViewModel>(supplier));
            }
        }
    }
}
