using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace PurchaseOrderTracker.Web.Features.Api.Account
{
    public class Login
    {
        public class Command : IRequest<CommandResult>
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class CommandResult
        {
            public CommandResult(bool succeeded)
            {
                Succeeded = succeeded;
            }

            public bool Succeeded { get; }
        }

        public class CreateHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly SignInManager<IdentityUser> _signInManager;

            public CreateHandler(SignInManager<IdentityUser> signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                var result = await _signInManager.PasswordSignInAsync(command.UserName, command.Password, true, false);
                return new CommandResult(result.Succeeded);
            }
        }
    }
}
