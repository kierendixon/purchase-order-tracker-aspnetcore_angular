using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PurchaseOrderTracker.Persistence;
using X.PagedList;
using static PurchaseOrderTracker.Application.Features.Supplier.Queries.EditQuery;

namespace PurchaseOrderTracker.Application.Features.Supplier.Queries;

public class EditQuery : IRequest<Result>
{
    public EditQuery(int? supplierId)
    {
        SupplierId = supplierId;
    }

    public int? SupplierId { get; }

    public class Result
    {
        public Result(SupplierViewModel supplier)
        {
            Suppliers.Add(supplier);
        }

        public Result(List<SupplierViewModel> suppliers)
        {
            Suppliers = suppliers;
        }

        public List<SupplierViewModel> Suppliers { get; } = new List<SupplierViewModel>();

        // TODO: rename or move to webapi layer
        public class SupplierViewModel
        {
            // public SupplierViewModel()
            // {
            // }
            public SupplierViewModel(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; }
            public string Name { get; }
        }
    }

    public class Handler : IRequestHandler<EditQuery, Result>
    {
        private readonly PoTrackerDbContext _context;
        //private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public Handler(PoTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(EditQuery request, CancellationToken cancellationToken)
        {
            // TODO: return a notfound error instead of exception if supplier not found
            IQueryable<Domain.Models.SupplierAggregate.Supplier> suppliersQueryable = _context.Supplier;
            if (request.SupplierId.HasValue)
            {
                suppliersQueryable = suppliersQueryable.Where(s => s.Id == request.SupplierId);
            }

            // TODO
            // System.ArgumentException: Expression of type
            // 'PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects.SupplierName'
            // cannot be used for constructor parameter of type 'System.String'
            // var suppliers = await suppliersQueryable
            //   .ProjectTo<Result.SupplierViewModel>(_configuration)
            //   .ToListAsync();
            var suppliers = await suppliersQueryable.ToListAsync();
            var supplersViewModels = _mapper.Map<List<Result.SupplierViewModel>>(suppliers);

            return new Result(supplersViewModels);
        }
    }
}
