using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;

namespace PurchaseOrderTracker.Application.Features.User.Commands
{
    public class CreateCommand : IRequest<CreateCommand.Result>
    {
        public CreateCommand(string username, string oneTimePassword, bool isAdmin)
        {
            UserName = username ?? throw new ArgumentNullException(nameof(username));
            OneTimePassword = oneTimePassword ?? throw new ArgumentNullException(nameof(oneTimePassword));
            IsAdmin = isAdmin;
        }

        public string UserName { get; }
        public string OneTimePassword { get; }
        public bool IsAdmin { get; }

        public class Result
        {
            public Result(bool succeeded, IEnumerable<IdentityError> errors)
            {
                Succeeded = succeeded;
                Errors = errors;
            }

            public Result(bool succeeded, string userId)
            {
                Succeeded = succeeded;
                UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            }

            public bool Succeeded { get; }
            public string UserId { get; }
            public IEnumerable<IdentityError> Errors { get; }
        }

        public class Handler : IRequestHandler<CreateCommand, Result>
        {
            private readonly IMapper _mapper;
            private readonly UserManager<ApplicationUser> _userManager;

            // todo should application have a dependency on AspNet.Identity ?
            public Handler(IMapper mapper, UserManager<ApplicationUser> userManager)
            {
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<Result> Handle(CreateCommand request, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<ApplicationUser>(request);
                var result = await _userManager.CreateAsync(user, request.OneTimePassword);

                return result.Succeeded
                    ? new Result(result.Succeeded, user.Id)
                    : new Result(result.Succeeded, result.Errors);
            }
        }
    }
}
