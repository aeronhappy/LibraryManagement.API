using AutoMapper;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.API.Profiles
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
