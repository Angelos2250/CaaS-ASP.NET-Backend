using AutoMapper;

namespace CaaS.Api.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Domain.Entities.Cart, DTOs.CartDto>();
            CreateMap<DTOs.ForCreation.CartForCreationDto, Domain.Entities.Cart>();
        }
    }
}
