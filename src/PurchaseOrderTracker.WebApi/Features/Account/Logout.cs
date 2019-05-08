using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PurchaseOrderTracker.Application.Identity;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Persistence.Identity;

namespace PurchaseOrderTracker.WebApi.Features.Account
{
    public class Logout
    {
        public class Command : IRequest
        {
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ICurrentUser _currentUser;

            public CommandHandler(UserManager<ApplicationUser> userManager, ICurrentUser currentUser)
            {
                _userManager = userManager;
                _currentUser = currentUser;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_currentUser.Username);
                if (user == null)
                {
                    throw new PurchaseOrderTrackerException($"Cannot find user. Username={_currentUser.Username}");
                }

                user.RefreshToken = null;
                user.RefreshTokenExpiresAt = null;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
