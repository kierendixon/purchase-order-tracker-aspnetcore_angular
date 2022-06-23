using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace PurchaseOrderTracker.Persistence;

public class MediatrQueryTrackingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly PoTrackerDbContext _context;

    public MediatrQueryTrackingBehavior(PoTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (request.GetType().FullName.Contains("Command"))
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        }

        return await next();
    }
}
