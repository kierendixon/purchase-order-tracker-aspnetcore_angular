using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    public class Edit
    {
        public class Query : IRequest<QueryResult>
        {
            public int? SupplierId { get; set; }
        }

        public class QueryResult
        {
            public QueryResult(SupplierViewModel supplier)
            {
                Suppliers.Add(supplier);
            }

            public QueryResult(List<SupplierViewModel> suppliers)
            {
                Suppliers = suppliers;
            }

            public List<SupplierViewModel> Suppliers { get; } = new List<SupplierViewModel>();

            public class SupplierViewModel
            {
                public SupplierViewModel() { }

                public SupplierViewModel(int id, string name)
                {
                    Id = id;
                    Name = name;
                }

                public int Id { get; }
                public string Name { get; }
            }
        }

        public class QueryHandler : IRequestHandler<Query, QueryResult>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(PoTrackerDbContext context, IConfigurationProvider configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public async Task<QueryResult> Handle(Query query, CancellationToken cancellationToken)
            {
                IQueryable<Domain.Models.SupplierAggregate.Supplier> suppliersQueryable = _context.Supplier;
                if (query.SupplierId != null)
                {
                    suppliersQueryable = suppliersQueryable.Where(s => s.Id == query.SupplierId);
                }

                var suppliers = await suppliersQueryable
                    .ProjectTo<QueryResult.SupplierViewModel>(_configuration)
                    .ToListAsync();

                return new QueryResult(suppliers);
            }
        }

        public class Command : IRequest<QueryResult>
        {
            public int? Id { get; set; }

            [Required]
            [StringLength(50, MinimumLength = 3)] // TODO: keep consistent with database
            public string Name { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, QueryResult>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(PoTrackerDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<QueryResult> Handle(Command command, CancellationToken cancellationToken)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var supplier = await _context.Supplier.FindAsync(command.Id);

                _mapper.Map(command, supplier);
                await _context.SaveChangesAsync();

                return new QueryResult(_mapper.Map<QueryResult.SupplierViewModel>(supplier));
            }
        }
    }
}