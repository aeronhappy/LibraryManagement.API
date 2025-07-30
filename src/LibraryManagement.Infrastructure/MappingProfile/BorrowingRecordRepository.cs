using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.MappingProfile
{
    public class BorrowingRecordRepository : IBorrowingRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowingRecordRepository(ApplicationDbContext context)
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
                .Where(b => b.BorrowerId == memberId && b.BookId == bookId)
                .Include(b => b.Borrower)
                .Include(b => b.Book)
                .FirstOrDefaultAsync();
        }
    }
}
