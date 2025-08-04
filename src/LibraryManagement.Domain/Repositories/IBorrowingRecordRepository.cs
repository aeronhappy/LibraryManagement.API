using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Repositories
{
   public interface IBorrowingRecordRepository
    {
        Task<BorrowingRecord?> GetByIdAsync(BorrowingRecordId id);
        Task<BorrowingRecord?> GetByMemberAndBookIdAsync(BookId id, MemberId memberId);
        Task<BorrowingRecord?> GetLatestBorrowedRecordByBookId(BookId id);
        Task<List<BorrowingRecord>> GetAllBorrowingRecord();
        Task AddAsync(BorrowingRecord borrowingRecord);
    }
}
