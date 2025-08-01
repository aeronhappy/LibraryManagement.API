using AutoMapper;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.API.Profiles
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
