using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;
using PurchaseOrderTracker.Identity.Persistence;
using X.PagedList;

namespace PurchaseOrderTracker.Application.Features.User.Queries
{
    public class GetUsersQuery : IRequest<GetUsersQuery.Result>
    {
        public GetUsersQuery(int? pageSize, int? pageNumber)
        {
            PageSize = pageSize ?? 5;
            PageNumber = pageNumber ?? 1;
        }

        public int PageSize { get; }
        public int PageNumber { get; }

        public class Result
        {
            public Result(PagedList<ApplicationUser> pagedList)
            {
                PagedList = pagedList;
            }

            public PagedList<ApplicationUser> PagedList { get; }
        }

        public class Handler : IRequestHandler<GetUsersQuery, Result>
        {
            private readonly IdentityDbContext _context;

            public Handler(IdentityDbContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                //// Need to call ToList() which fetches all fields from
                //// the database instead of projecting because of a bug in EF Core 2.1
                //// https://github.com/aspnet/EntityFrameworkCore/issues/13546
                ///// ?????
                var paginatedUsers = new PagedList<ApplicationUser>(_context.Users, request.PageNumber, request.PageSize);

                return new Result(paginatedUsers);
            }
        }
    }
}
