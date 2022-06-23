using AutoMapper;
using PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries;

namespace PurchaseOrderTracker.Application.Features.PurchaseOrder;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Models.PurchaseOrderAggregate.PurchaseOrder, EditQuery.Result>()
            .ForMember(dest => dest.SupplierOptions, opt => opt.Ignore())
            .ForMember(dest => dest.ShipmentOptions, opt => opt.Ignore())
            .ForMember(dest => dest.CurrentState, opt => opt.MapFrom(src => src.Status.CurrentState));
    }
}
