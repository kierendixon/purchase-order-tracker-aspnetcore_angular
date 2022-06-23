using AutoMapper;
using PurchaseOrderTracker.Application.Features.PurchaseOrder.Queries;
using PurchaseOrderTracker.Application.String;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models;
using static PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models.EditLineItemsQueryResultDto;
using static PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models.InquiryQueryResultDto;

namespace PurchaseOrderTracker.WebApi.Features.PurchaseOrder;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PurchaseOrderLine, PurchaseOrderLineDto>();

        CreateMap<EditLineItemsQuery.Result, EditLineItemsQueryResultDto>();

        // AutoMapper error when using ProjectTo():
        // (when status param is named "statusCurrentState")
        //
        // System.ArgumentException: Expression of type 'PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.PurchaseOrderStatus+State' cannot be used for constructor parameter of type 'System.String'
        // Parameter name: arguments[4]
        // CreateMap<Domain.Models.PurchaseOrderAggregate.PurchaseOrder, Inquiry.Result.PurchaseOrderViewModel>()
        //    .ConstructUsing(src => new Inquiry.Result.PurchaseOrderViewModel(src.Id, src.OrderNo,
        //        src.CreatedDate, src.Supplier.Name, src.Status.Status.ToString()))
        //    .ForCtorParam("statusCurrentState", opt => opt.MapFrom(src => src.Status.Status.ToString()));

        // AutoMapper error when using ProjectTo():
        // (when status param is named "currentState")
        //
        // AutoMapper.AutoMapperMappingException: Unable to generate the instantiation expression for the constructor
        // Void .ctor(Int32, System.String, System.DateTime, System.String, System.String):
        // no expression could be mapped for constructor parameter 'System.String currentState'.
        // CreateMap<Domain.Models.PurchaseOrderAggregate.PurchaseOrder, Inquiry.Result.PurchaseOrderViewModel>()
        //    .ConstructUsing(src => new Inquiry.Result.PurchaseOrderViewModel(src.Id, src.OrderNo,
        //        src.CreatedDate, src.Supplier.Name, src.Status.CurrentState.ToString()))
        //    .ForCtorParam("currentState", opt => opt.MapFrom(src => src.Status.CurrentState.ToString()));
        CreateMap<Domain.Models.PurchaseOrderAggregate.PurchaseOrder, PurchaseOrderDto>()
            .ForCtorParam(
                nameof(PurchaseOrderDto.Status).FirstCharacterToLower(),
                opt => opt.MapFrom(src => src.Status.CurrentState.ToString()));
    }
}
