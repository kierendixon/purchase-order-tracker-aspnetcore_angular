using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PurchaseOrderTracker.Persistence;
using X.PagedList;
using static PurchaseOrderTracker.Application.Features.Supplier.Queries.InquiryQuery;

namespace PurchaseOrderTracker.Application.Features.Supplier.Queries;

public class InquiryQuery : IRequest<Result>
{
    public InquiryQuery(int? pageSize, int? pageNumber, QueryType inquiryQueryType)
    {
        PageSize = pageSize ?? 5;
        PageNumber = pageNumber ?? 1;
        InquiryQueryType = inquiryQueryType;
    }

    public enum QueryType
    {
        All
    }

    public int PageSize { get; }
    public int PageNumber { get; }
    public QueryType InquiryQueryType { get; }

    public class Result
    {
        public Result(PagedList<Domain.Models.SupplierAggregate.Supplier> pagedList)
        {
            PagedList = pagedList;
        }

        public PagedList<Domain.Models.SupplierAggregate.Supplier> PagedList { get; }
    }

    public class Handler : IRequestHandler<InquiryQuery, Result>
    {
        private readonly PoTrackerDbContext _context;

        public Handler(PoTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(InquiryQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _context.Supplier.ToListAsync();

            // TODO: projectTo() is unnecessary if we are returning the same type
            // define subset classes in the application layer and project to that
            // name as Minimal instead of ViewModel
            var paginatedSuppliers = new PagedList<Domain.Models.SupplierAggregate.Supplier>(
                suppliers, request.PageNumber, request.PageSize);

            return new Result(paginatedSuppliers);
        }
    }
}
