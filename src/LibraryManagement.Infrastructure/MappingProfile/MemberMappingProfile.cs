namespace LibraryManagement.API.Profiles
{
    using AutoMapper;
    using LibraryManagement.Application.Response;
    using LibraryManagement.Domain.Entities;

    public class MemberMappingProfile : Profile
    {
    
        public MemberMappingProfile()
        {
            CreateMap<Member, MemberResponse>()
                 .ForMember(m => m.Id, option => option.MapFrom(m => m.Id.Value));

            CreateMap<Member, BorrowerResponse>()
               .ForMember(r => r.Id, option => option.MapFrom(r => r.Id.Value));
        }
    }
}