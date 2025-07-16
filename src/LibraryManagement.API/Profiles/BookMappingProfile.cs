using AutoMapper;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Response;

namespace LibraryManagement.API.Profiles
{

    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, BookResponse>();
        }
    }
}
