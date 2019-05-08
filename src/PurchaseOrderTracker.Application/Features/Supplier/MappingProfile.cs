using AutoMapper;
using PurchaseOrderTracker.Application.Features.Supplier.Commands;
using PurchaseOrderTracker.Application.Features.Supplier.Queries;
using PurchaseOrderTracker.Application.String;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.Application.Features.Supplier
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCommand, Domain.Models.SupplierAggregate.Supplier>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Domain.Models.SupplierAggregate.Supplier, EditQuery.Result.SupplierViewModel>()
                .ForCtorParam(
                    nameof(EditQuery.Result.SupplierViewModel.Name).FirstCharacterToLower(),
                    opt => opt.MapFrom(src => src.Name.Value));

            CreateMap<ProductCategory, EditProductCategoriesQuery.Result.CategoryViewModel>();

            CreateMap<EditCommand, Domain.Models.SupplierAggregate.Supplier>()
                .ForSourceMember(src => src.SupplierId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName));

            CreateMap<Product, EditProductsQuery.Result.ProductViewModel>()
                .ForCtorParam(
                    nameof(EditProductsQuery.Result.ProductViewModel.ProductId).FirstCharacterToLower(),
                    opt => opt.MapFrom(src => src.Id))
                .ForCtorParam(
                    "productCode",
                    opt => opt.MapFrom(src => src.ProductCode.Value))
                .ForCtorParam(
                    nameof(EditProductsQuery.Result.ProductViewModel.Name).FirstCharacterToLower(),
                    opt => opt.MapFrom(src => src.Name.Value));
        }
    }
}
