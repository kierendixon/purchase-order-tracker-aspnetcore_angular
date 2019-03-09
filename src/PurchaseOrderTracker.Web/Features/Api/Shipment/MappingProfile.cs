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

            // AutoMapper error when using ProjectTo():
            //
            // (when currentState param is named "currentState")
            // System.ArgumentException: Type 'PurchaseOrderTracker.Web.Features.Api.Shipment.Inquiry+Result+ShipmentViewModel' does not have a default constructor
            // Parameter name: type
            //
            // (when currentState param is named "statusCurrentState")
            // System.ArgumentException: Static method requires null instance, non-static method requires non-null instance.
            //
            // (when removing params mapped from methods in the source object [IsDelayed, IsDelayedMoreThan7Days, IsScheduledForDeliveryToday])
            // System.ArgumentException: Expression of type 'System.Nullable`1[System.DateTime]' cannot be used for constructor parameter of type 'System.DateTime'
            // Parameter name: arguments[3]
            CreateMap<Domain.Models.ShipmentAggregate.Shipment, Inquiry.Result.ShipmentViewModel>()
                .ConstructUsing(src => new Inquiry.Result.ShipmentViewModel(src.Id, src.TrackingId, src.Company,
                src.EstimatedArrivalDate.Value, src.Comments, src.ShippingCost.Value,
                src.Status.CurrentState.ToString(), src.DestinationAddress, src.IsDelayed(),
                src.IsDelayedMoreThan7Days(), src.IsScheduledForDeliveryToday()));

            // AutoMapper error when using ProjectTo():
            // System.ArgumentException: Static method requires null instance, non-static method requires non-null instance.
            // CreateMap<Domain.Models.ShipmentAggregate.Shipment, Inquiry.Result.ShipmentViewModel>()
            //    .ForCtorParam(nameof(Inquiry.Result.ShipmentViewModel.Status).FirstCharacterToLower(),
            //        opt => opt.MapFrom(src => src.Status.Status));
        }
    }
}
