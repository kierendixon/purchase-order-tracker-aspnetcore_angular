using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PurchaseOrderTracker.Persistence;
using X.PagedList;

namespace PurchaseOrderTracker.Application.Features.User.Queries;

public class GetUsersQuery : IRequest<GetUsersQuery.Result>
{
    public GetUsersQuery(int? pageSize, string filter, int? pageNumber)
    {
        PageSize = pageSize ?? 5;
        Filter = filter;
        PageNumber = pageNumber ?? 1;

        // TODO
        //if (PageSize is < 5 or > 100)
        //{
        //}

        //if (PageNumber < 1)
        //{
        //}
    }

    public int PageSize { get; }
    public int PageNumber { get; }
    public string Filter { get; }

    public class Result
    {
        public Result(PagedList<ResultUser> pagedList)
        {
            PagedList = pagedList;
        }

        public PagedList<ResultUser> PagedList { get; }

        public class ResultUser
        {
            // todo should bother with argument validaiton?
            public ResultUser(string id, string userName, bool isAdmin, DateTimeOffset? lockoutEnd)
            {
                Id = id;
                IsAdmin = isAdmin;
                UserName = userName;
                LockoutEnd = lockoutEnd?.LocalDateTime;
            }

            public string Id { get; }
            public string UserName { get; }
            public bool IsAdmin { get; }
            public DateTime? LockoutEnd { get; }
        }
    }

    public class Handler : IRequestHandler<GetUsersQuery, Result>
    {
        private readonly IdentityDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IdentityDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            //// Need to call ToList() which fetches all fields from
            //// the database instead of projecting because of a bug in EF Core 2.1
            //// https://github.com/aspnet/EntityFrameworkCore/issues/13546
            ///// ????? TODO
            // todo use ProjectToPagedList
            // and fix whatever issues it has (have they been fixed in soruce libraries??)
            var users = (await _context.Users.ToListAsync()).AsQueryable();
            if (request.Filter != null)
            {
                users = users.Where(u => u.UserName.Contains(request.Filter, StringComparison.OrdinalIgnoreCase));
            }

            var paginatedUsers = new PagedList<Result.ResultUser>(_mapper.Map<Result.ResultUser[]>(users), request.PageNumber, request.PageSize);

            return new Result(paginatedUsers);
        }
    }
}
