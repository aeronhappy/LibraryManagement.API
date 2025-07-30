namespace LibraryManagement.API.Profiles
{
    using AutoMapper;
    using LibraryManagement.Application.Response;
    using LibraryManagement.Domain.Entities;

    public class MemberMappingProfile : Profile
    {
    
        public MemberMappingProfile()
        {
            CreateMap<Member, MemberResponse>();
            CreateMap<Member, BorrowerResponse>();
        }
    }
}