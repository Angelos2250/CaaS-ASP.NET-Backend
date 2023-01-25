using AutoMapper;

namespace CaaS.Api.Profiles
{
    public class ShopOwnerProfile : Profile
    {
        public ShopOwnerProfile()
        {
            CreateMap<Domain.Entities.ShopOwner, DTOs.ShopOwnerDto>();
            CreateMap<DTOs.ForCreation.ShopOwnerForCreationDto, Domain.Entities.ShopOwner>();
        }
    }
}
