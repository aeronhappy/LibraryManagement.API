using AutoMapper;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Response;

namespace LibraryManagement.API.Profiles
{

    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleResponse>(); 
            CreateMap<RoleResponse, Role>();
        }
    }
}
