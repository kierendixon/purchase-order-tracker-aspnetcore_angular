using AutoMapper;
using PurchaseOrderTracker.Application.Features.Shipment.Commands;
using PurchaseOrderTracker.Application.Features.Shipment.Queries;

namespace PurchaseOrderTracker.Application.Features.Shipment
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Models.ShipmentAggregate.Shipment, EditQuery.Result>()
                .ForMember(dest => dest.CurrentState, opt => opt.MapFrom(src => src.Status.CurrentState));

            CreateMap<EditCommand, Domain.Models.ShipmentAggregate.Shipment>()
                .ForSourceMember(src => src.ShipmentId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseOrders, opt => opt.Ignore());

            CreateMap<CreateCommand, Domain.Models.ShipmentAggregate.Shipment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseOrders, opt => opt.Ignore());

            CreateMap<Domain.Models.ShipmentAggregate.Shipment, CreateCommand>();
        }
    }
}
