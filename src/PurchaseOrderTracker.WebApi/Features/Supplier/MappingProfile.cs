using AutoMapper;
using PurchaseOrderTracker.WebApi.Features.Supplier.Models;

namespace PurchaseOrderTracker.WebApi.Features.Supplier;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Models.SupplierAggregate.Supplier, InquiryQueryResultDto.SupplierDto>();
    }
}

