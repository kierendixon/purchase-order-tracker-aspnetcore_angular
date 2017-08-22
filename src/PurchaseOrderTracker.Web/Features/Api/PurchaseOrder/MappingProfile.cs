using AutoMapper;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Infrastructure;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Models.PurchaseOrderAggregate.PurchaseOrder, Edit.QueryResult>()
                .ForMember(dest => dest.SupplierOptions, opt => opt.Ignore())
                .ForMember(dest => dest.ShipmentOptions, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentState, opt => opt.MapFrom(src => src.Status.CurrentState));

            CreateMap<Domain.Models.PurchaseOrderAggregate.PurchaseOrder, Inquiry.Result.PurchaseOrderViewModel>()
                .ForCtorParam(nameof(Inquiry.Result.PurchaseOrderViewModel.CurrentState).FirstCharacterToLower(),
                    opt => opt.MapFrom(src => src.Status.CurrentState));

            CreateMap<PurchaseOrderLine, EditLineItems.Result.PurchaseOrderLineViewModel>();
            CreateMap<Product, EditLineItems.Result.PurchaseOrderLineViewModel>();
        }
    }
}