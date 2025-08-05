using AutoMapper;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.MappingProfile
{
    

    public class BorrowingRecordMappingProfile : Profile
    {
        public BorrowingRecordMappingProfile()
        {
            CreateMap<BorrowingRecord, BorrowRecordResponse>()
               .ForMember(r => r.Id, option => option.MapFrom(r => r.Id.Value));

            CreateMap<BorrowingRecord, MemberBorrowRecordResponse>()
              .ForMember(r => r.Id, option => option.MapFrom(r => r.Id.Value));

            CreateMap<BorrowingRecord, BookBorrowRecordResponse>()
             .ForMember(r => r.Id, option => option.MapFrom(r => r.Id.Value));


        }
    }
}
