using AutoMapper;
using PurchaseOrderTracker.Application.Features.User.Commands;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;

namespace PurchaseOrderTracker.Application.Features.User;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCommand, ApplicationUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
            .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore());
    }
}
