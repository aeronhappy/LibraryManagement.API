using AutoMapper;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.API.Profiles
{

    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, BookResponse>()
                .ForMember(b => b.Id, option=> option.MapFrom(b=>b.Id.Value));
        }
    }
}
