using Borrowing.Domain.Entities;
using Borrowing.Domain.Repositories;
using Borrowing.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Borrowing.Infrastructure.Data.Repositories
{
    public class BorrowingRecordRepository : IBorrowingRecordRepository
    {
        private readonly BorrowingDbContext _context;

        public BorrowingRecordRepository(BorrowingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BorrowingRecord borrowingRecord)
        {
            await _context.BorrowingRecords.AddAsync(borrowingRecord);
        }

        public async Task<BorrowingRecord?> GetByIdAsync(BorrowingRecordId id)
        {
            return await _context.BorrowingRecords
                .Where(br => br.Id == id)
                .Include(br => br.Book)
                .Include(br => br.Borrower)
                .FirstOrDefaultAsync();
        }

        public async Task<BorrowingRecord?> GetByMemberAndBookIdAsync(BookId bookId, MemberId memberId)
        {
            return await _context.BorrowingRecords
                .Where(b => b.BorrowerId == memberId && b.BookId == bookId && b.DateReturned == null)
                .Include(b => b.Borrower)
                .Include(b => b.Book)
                .FirstOrDefaultAsync();
        }

        /// Request
        public async Task RequestAsync(BorrowingRequest borrowingRequest)
        {
            await _context.BorrowingRequests.AddAsync(borrowingRequest);
        }


        public async Task<BorrowingRequest?> GetBorrowingRequestAsyncById(BorrowingRequestId id)
        {
            return await _context.BorrowingRequests
                .Where(br => br.Id == id)
                .Include(br => br.Book)
                .Include(br => br.Borrower)
                .FirstOrDefaultAsync();
        }
    }
}
