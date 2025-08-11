using AutoMapper;
using Identity.Application.Response;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.MappingProfile
{

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponse>()
                .ForMember(u => u.Id, option => option.MapFrom(u => u.Id.Value));

        }
    }
}
