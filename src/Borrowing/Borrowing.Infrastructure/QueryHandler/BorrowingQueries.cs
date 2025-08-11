using AutoMapper;
using AutoMapper.QueryableExtensions;
using Borrowing.Application.Queries;
using Borrowing.Application.Response;
using Borrowing.Domain.Entities;
using Borrowing.Domain.ValueObjects;
using Borrowing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Borrowing.Infrastructure.QueryHandler
{
    public class BorrowingQueries : IBorrowingQueries
    {
        private readonly BorrowingDbContext _context;
        private readonly IMapper _mapper;

        public BorrowingQueries(BorrowingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BorrowRecordResponse>> GetAllBorrowRecordsAsync(string searchText, DateTime? dataTime)
        {
            IQueryable<BorrowingRecord> query = _context.BorrowingRecords.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {

                var loweredSearchText = searchText.ToLower();
                query = query.Where(br =>
                                    br.Book.Title.ToLower().Contains(loweredSearchText) ||
                                    br.Book.Author.ToLower().Contains(loweredSearchText) ||
                                    br.Borrower.Name.ToLower().Contains(loweredSearchText) ||
                                    br.Borrower.Email.ToLower().Contains(loweredSearchText));
            }

            if (dataTime.HasValue)
            {
                query = query.Where(record => record.DateBorrowed.Date == dataTime.Value.Date);
            }

            return await query.OrderByDescending(q => q.DateBorrowed)
                              .ProjectTo<BorrowRecordResponse>(_mapper.ConfigurationProvider).ToListAsync();


        }

        public async Task<List<BorrowRecordResponse>> GetUnreturnedBorrowRecordsAsync(string searchText, DateTime? dataTime)
        {
            IQueryable<BorrowingRecord> query = _context.BorrowingRecords.Where(br => br.DateReturned == null);

            if (!string.IsNullOrWhiteSpace(searchText))
            {

                var loweredSearchText = searchText.ToLower();
                query = query.Where(br =>
                                    br.Book.Title.ToLower().Contains(loweredSearchText) ||
                                    br.Book.Author.ToLower().Contains(loweredSearchText) ||
                                    br.Borrower.Name.ToLower().Contains(loweredSearchText) ||
                                    br.Borrower.Email.ToLower().Contains(loweredSearchText));
            }

            if (dataTime.HasValue)
            {
                query = query.Where(record => record.DateBorrowed.Date == dataTime.Value.Date);
            }

            return await query.OrderByDescending(q => q.DateBorrowed)
                              .ProjectTo<BorrowRecordResponse>(_mapper.ConfigurationProvider).ToListAsync();


        }


        public async Task<List<BorrowRecordResponse>> GetReturnedBorrowRecordsAsync(string searchText, DateTime? dataTime)
        {
            IQueryable<BorrowingRecord> query = _context.BorrowingRecords.Where(br => br.DateReturned != null);

            if (!string.IsNullOrWhiteSpace(searchText))
            {

                var loweredSearchText = searchText.ToLower();
                query = query.Where(br =>
                                    br.Book.Title.ToLower().Contains(loweredSearchText) ||
                                    br.Book.Author.ToLower().Contains(loweredSearchText) ||
                                    br.Borrower.Name.ToLower().Contains(loweredSearchText) ||
                                    br.Borrower.Email.ToLower().Contains(loweredSearchText));
            }

            if (dataTime.HasValue)
            {
                query = query.Where(record => record.DateBorrowed.Date == dataTime.Value.Date);
            }

            return await query.OrderByDescending(q => q.DateBorrowed)
                              .ProjectTo<BorrowRecordResponse>(_mapper.ConfigurationProvider).ToListAsync();


        }

        public async Task<BorrowRecordResponse?> GetBorrowingRecordById(Guid recordId)
        {
            var borrowingRecord = await _context.BorrowingRecords
                .Where(br => br.Id == new BorrowingRecordId(recordId))
                .ProjectTo<BorrowRecordResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return borrowingRecord;
        }
    }
}
