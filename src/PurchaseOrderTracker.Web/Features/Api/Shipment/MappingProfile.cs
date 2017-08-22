using AutoMapper;
using PurchaseOrderTracker.Infrastructure;

namespace PurchaseOrderTracker.Web.Features.Api.Shipment
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Models.ShipmentAggregate.Shipment, Edit.QueryResult>()
                .ForMember(dest => dest.CurrentState, opt => opt.MapFrom(src => src.Status.CurrentState));

            CreateMap<Edit.Command, Domain.Models.ShipmentAggregate.Shipment>()
                .ForSourceMember(src => src.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseOrders, opt => opt.Ignore());

            CreateMap<Create.Command, Domain.Models.ShipmentAggregate.Shipment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseOrders, opt => opt.Ignore());

            CreateMap<Domain.Models.ShipmentAggregate.Shipment, Create.Command>();

            CreateMap<Domain.Models.ShipmentAggregate.Shipment, Inquiry.Result.ShipmentViewModel>()
                .ForCtorParam(nameof(Inquiry.Result.ShipmentViewModel.CurrentState).FirstCharacterToLower(),
                    opt => opt.MapFrom(src => src.Status.CurrentState));
        }
    }
}
