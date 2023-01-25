using AutoMapper;
using Domain.Entities;

namespace CaaS.Api.Profiles
{
    public class ResultProfile : Profile
    {
        public ResultProfile()
        {
            CreateMap<Result, DTOs.ResultDTO>();
        }
    }
}
