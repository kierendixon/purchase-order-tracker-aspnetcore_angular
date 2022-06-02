using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;

namespace PurchaseOrderTracker.Application.Features.User.Commands
{
    public class DeleteCommand : IRequest<DeleteCommand.Result>
    {
        public DeleteCommand(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public string Id { get; }

        public class Result
        {
            public Result(bool succeeded, IEnumerable<IdentityError> errors)
            {
                Succeeded = succeeded;
                Errors = errors;
            }

            public bool Succeeded { get; }
            public IEnumerable<IdentityError> Errors { get; }
        }

        public class Handler : IRequestHandler<DeleteCommand, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;

            // todo should application have a dependency on AspNet.Identity ?
            public Handler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Result> Handle(DeleteCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.Id);
                var result = await _userManager.DeleteAsync(user);
                return new Result(result.Succeeded, result.Errors);
            }
        }
    }
}
