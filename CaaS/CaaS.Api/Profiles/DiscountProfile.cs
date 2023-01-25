using AutoMapper;

namespace CaaS.Api.Profiles
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Domain.Entities.Discount, DTOs.DiscountDto>();
            CreateMap<DTOs.ForCreation.DiscountForCreationDto, Domain.Entities.Discount>();
        }
    }
}
