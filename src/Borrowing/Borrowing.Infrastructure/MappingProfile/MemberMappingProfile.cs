using AutoMapper;
using Borrowing.Application.Response;
using Borrowing.Domain.Entities;


namespace Borrowing.Infrastructure.MappingProfile
{


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