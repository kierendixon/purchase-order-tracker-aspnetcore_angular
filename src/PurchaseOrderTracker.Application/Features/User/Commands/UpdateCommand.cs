using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;

namespace PurchaseOrderTracker.Application.Features.User.Commands
{
    public class UpdateCommand : IRequest<UpdateCommand.Result>
    {
        public UpdateCommand(string id, string username, bool isAdmin, DateTime? lockoutEnd)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            UserName = username ?? throw new ArgumentNullException(nameof(username));
            IsAdmin = isAdmin;
            LockoutEnd = lockoutEnd;
        }

        public string Id { get; }
        public string UserName { get; }
        public bool IsAdmin { get; }
        public DateTime? LockoutEnd { get; }

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

        public class Handler : IRequestHandler<UpdateCommand, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;

            // todo should application have a dependency on AspNet.Identity ?
            public Handler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Result> Handle(UpdateCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.Id);
                user.IsAdmin = request.IsAdmin;
                user.LockoutEnd = request.LockoutEnd;
                user.UserName = request.UserName;
                // todo
                // user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
                var result = await _userManager.UpdateAsync(user);
                return new Result(result.Succeeded, result.Errors);
            }
        }
    }
}
