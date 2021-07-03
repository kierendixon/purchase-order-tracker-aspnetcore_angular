using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;
using PurchaseOrderTracker.Persistence;

namespace PurchaseOrderTracker.Application.Features.User.Commands
{
    public class DeleteCommand : IRequest<DeleteCommand.Result>
    {
        public DeleteCommand(string id)
        {
            Id = id?? throw new ArgumentNullException(nameof(id));
        }

        public string Id { get; }

        public class Result
        {
            public Result(bool succeeded, IEnumerable<IdentityError> errors)
            {
                Succeeded = succeeded;
                Errors = errors;
            }

            public bool Succeeded { get;  }
            public IEnumerable<IdentityError> Errors { get; }
        }

        public class Handler : IRequestHandler<DeleteCommand, Result>
        {
            private readonly IMapper _mapper;
            private readonly UserManager<ApplicationUser> _userManager;

            // todo should application have a dependency on AspNet.Identity ?
            public Handler(PoTrackerDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
            {
                _mapper = mapper;
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
