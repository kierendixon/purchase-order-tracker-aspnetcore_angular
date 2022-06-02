using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.Identity.Features
{
    [Route("[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IMediator Mediator { get; }
        protected IMapper Mapper { get; }

        protected BaseController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }
    }
}
