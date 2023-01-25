using AutoMapper;

namespace CaaS.Api.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Domain.Entities.Customer, DTOs.CustomerDto>();
            CreateMap<DTOs.ForCreation.CustomerForCreationDto, Domain.Entities.Customer>();
        }
    }
}
