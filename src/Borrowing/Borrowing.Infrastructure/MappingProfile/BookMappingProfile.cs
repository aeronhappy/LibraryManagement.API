using AutoMapper;
using Borrowing.Application.Response;
using Borrowing.Domain.Entities;

namespace Borrowing.Infrastructure.MappingProfile
{

    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, BookResponse>()
                .ForMember(b => b.Id, option => option.MapFrom(b => b.Id.Value));
            CreateMap<Book, BookBorrowedResponse>()
                 .ForMember(b => b.Id, option => option.MapFrom(b => b.Id.Value));
        }
    }
}
