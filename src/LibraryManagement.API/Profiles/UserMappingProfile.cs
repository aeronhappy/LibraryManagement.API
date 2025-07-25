using AutoMapper;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Response;

namespace LibraryManagement.API.Profiles
{

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponse>();
        }
    }
}
