using AutoMapper;
using Borrowing.Application.Response;
using Borrowing.Domain.Entities;

namespace Borrowing.Infrastructure.MappingProfile
{


    public class BorrowingRequestMappingProfile : Profile
    {
        public BorrowingRequestMappingProfile()
        {
            CreateMap<BorrowingRequest, BorrowRequestResponse>()
               .ForMember(r => r.Id, option => option.MapFrom(r => r.Id.Value));
            


        }

      
    }
}
