using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;

namespace PurchaseOrderTracker.Identity.Features.Users;

public class CreateCommand : IRequest<CreateCommand.Result>
{
    // TODO ctor validation
    public CreateCommand(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; }
    public string Password { get; }

    public class Result
    {
        public Result(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }

    public class Handler : IRequestHandler<CreateCommand, Result>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Handler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser(request.Username);
            var result = await _userManager.CreateAsync(user, request.Password);

            //if (!result.Succeeded)
            //{
            //    var errors = string.Join(",", result.Errors.Select(e => e.Description));
            //    throw new PurchaseOrderTrackerException($"Failed to create user while initializing identity database: {errors}");
            //}

            // handle duplicate username
            return new Result(result.Succeeded ? user.Id : null);
        }
    }
}
