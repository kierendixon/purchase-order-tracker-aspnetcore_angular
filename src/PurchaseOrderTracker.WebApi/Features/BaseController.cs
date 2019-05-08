using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PurchaseOrderTracker.WebApi.Features
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;

        protected BaseController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
    }
}
