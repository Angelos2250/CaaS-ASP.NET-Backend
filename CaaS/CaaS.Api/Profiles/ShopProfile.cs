using AutoMapper;

namespace CaaS.Api.Profiles
{
    public class ShopProfile : Profile
    {
        public ShopProfile()
        {
            CreateMap<Domain.Entities.Shop, DTOs.ShopDto>();
            CreateMap<DTOs.ForCreation.ShopForCreationDto, Domain.Entities.Shop>();
            CreateMap<DTOs.ShopDto, Domain.Entities.Shop>();
        }
    }
}
