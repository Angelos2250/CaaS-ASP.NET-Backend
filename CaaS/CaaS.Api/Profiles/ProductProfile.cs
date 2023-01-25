using AutoMapper;

namespace CaaS.Api.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Domain.Entities.Product, DTOs.ProductDto>();
            CreateMap<Domain.Entities.ProductWithQty, DTOs.ProductDto>();
            CreateMap<DTOs.ForCreation.ProductForCreationDto, Domain.Entities.Product>();
        }
    }
}
