using AutoMapper;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Infrastructure;

namespace PurchaseOrderTracker.Web.Features.Api.Supplier
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, Domain.Models.SupplierAggregate.Supplier>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Domain.Models.SupplierAggregate.Supplier, Edit.QueryResult.SupplierViewModel>();
            CreateMap<Edit.Command, Domain.Models.SupplierAggregate.Supplier>().ForSourceMember(src => src.Id, opt => opt.Ignore());
            CreateMap<Domain.Models.SupplierAggregate.Supplier, Inquiry.Result.SupplierViewModel>();
            CreateMap<ProductCategory, EditProductCategories.Result.CategoryViewModel>();

            CreateMap<CreateProduct.Command, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<Product, EditProducts.Result.ProductViewModel>()
                .ForCtorParam(nameof(EditProducts.Result.ProductViewModel.ProductId).FirstCharacterToLower(),
                opt => opt.MapFrom(src => src.Id));
        }
    }
}