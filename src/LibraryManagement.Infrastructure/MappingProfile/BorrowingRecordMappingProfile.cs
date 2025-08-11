using AutoMapper;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Infrastructure.MappingProfile
{


    public class BorrowingRecordMappingProfile : Profile
    {
        public BorrowingRecordMappingProfile()
        {
            CreateMap<BorrowingRecord, BorrowRecordResponse>()
               .ForMember(r => r.Id, option => option.MapFrom(r => r.Id.Value))
               .ForMember(r => r.DateBorrowed, opt => opt.MapFrom(r => ConvertToUtcOffset(r.DateBorrowed)))
               .ForMember(r => r.DateOverdue, opt => opt.MapFrom(r => ConvertToUtcOffset(r.DateOverdue)))
               .ForMember(r => r.DateReturned, opt => opt.MapFrom(r => ConvertNullableToUtcOffset(r.DateReturned)));

            CreateMap<BorrowingRecord, MemberBorrowRecordResponse>()
              .ForMember(r => r.Id, option => option.MapFrom(r => r.Id.Value))
              .ForMember(r => r.DateBorrowed, opt => opt.MapFrom(r => ConvertToUtcOffset(r.DateBorrowed)))
              .ForMember(r => r.DateOverdue, opt => opt.MapFrom(r => ConvertToUtcOffset(r.DateOverdue)))
              .ForMember(r => r.DateReturned, opt => opt.MapFrom(r => ConvertNullableToUtcOffset(r.DateReturned)));


            CreateMap<BorrowingRecord, BookBorrowRecordResponse>()
             .ForMember(r => r.Id, option => option.MapFrom(r => r.Id.Value))
               .ForMember(r => r.DateBorrowed, opt => opt.MapFrom(r => ConvertToUtcOffset(r.DateBorrowed)))
               .ForMember(r => r.DateOverdue, opt => opt.MapFrom(r => ConvertToUtcOffset(r.DateOverdue)))
               .ForMember(r => r.DateReturned, opt => opt.MapFrom(r => ConvertNullableToUtcOffset(r.DateReturned)));



        }

        public static DateTimeOffset ConvertToUtcOffset(DateTime dateTime)
        {
            return new DateTimeOffset(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
        }

        public static DateTimeOffset? ConvertNullableToUtcOffset(DateTime? dateTime)
        {
            return dateTime.HasValue
                ? new DateTimeOffset(DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc))
                : (DateTimeOffset?)null;
        }
    }
}
