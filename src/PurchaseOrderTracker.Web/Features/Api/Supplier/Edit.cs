using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            public int? Id { get; set; }
        }

        public enum ViewType { IdName }

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

        public class QueryHandler : IAsyncRequestHandler<Query, QueryResult>
        {
            private readonly PoTrackerDbContext _context;

            public QueryHandler(PoTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<QueryResult> Handle(Query query)
            {
                IQueryable<Domain.Models.SupplierAggregate.Supplier> suppliersQueryable = _context.Supplier;
                if (query.Id != null)
                {
                    suppliersQueryable = suppliersQueryable.Where(s => s.Id == query.Id);
                }

                var suppliers = await suppliersQueryable.ProjectTo<QueryResult.SupplierViewModel>().ToListAsync();
                return new QueryResult(suppliers);
            }
        }

        public class Command : IRequest<QueryResult>
        {
            public int? Id { get; set; }

            [Required]
            [StringLength(50, MinimumLength = 3)]
            public string Name { get; set; }
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
                var supplier = await _context.Supplier.FindAsync(command.Id);

                Mapper.Map(command, supplier);
                await _context.SaveChangesAsync();

                return new QueryResult(Mapper.Map<QueryResult.SupplierViewModel>(supplier));
            }
        }
    }
}