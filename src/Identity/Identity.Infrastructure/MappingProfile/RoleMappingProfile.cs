using AutoMapper;
using Identity.Application.Response;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.MappingProfile
{

    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleResponse>()
                .ForMember(r => r.Id, option => option.MapFrom(r => r.Id.Value));
        }
    }
}
