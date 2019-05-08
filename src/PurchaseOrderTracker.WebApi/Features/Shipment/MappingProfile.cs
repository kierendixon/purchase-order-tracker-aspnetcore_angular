using AutoMapper;
using PurchaseOrderTracker.Application.Features.Shipment.Commands;
using PurchaseOrderTracker.WebApi.Features.Shipment.Models;

namespace PurchaseOrderTracker.WebApi.Features.Shipment
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCommandDto, CreateCommand>();

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
            CreateMap<Domain.Models.ShipmentAggregate.Shipment, InquiryQueryResultDto.ShipmentDto>()
                .ConstructUsing(src => new InquiryQueryResultDto.ShipmentDto(src.Id, src.TrackingId, src.Company,
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
