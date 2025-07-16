using LibraryManagement.API.DTOs.Response;

namespace LibraryManagement.API.Profiles
{
    using AutoMapper;
    using LibraryManagement.API.Datas.Models;

    public class MemberMappingProfile : Profile
    {
    
        public MemberMappingProfile()
        {
            CreateMap<Member, MemberResponse>();
            CreateMap<Member, BorrowerResponse>();
        }
    }
}