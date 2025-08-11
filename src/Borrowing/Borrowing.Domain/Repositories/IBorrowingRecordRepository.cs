using Borrowing.Domain.Entities;
using Borrowing.Domain.ValueObjects;

namespace Borrowing.Domain.Repositories
{
    public interface IBorrowingRecordRepository
    {
        Task<BorrowingRecord?> GetByIdAsync(BorrowingRecordId id);
        Task<BorrowingRecord?> GetByMemberAndBookIdAsync(BookId id, MemberId memberId);
        Task AddAsync(BorrowingRecord borrowingRecord);


        ///Request
        Task RequestAsync(BorrowingRequest borrowingRecord);
        Task<BorrowingRequest?> GetBorrowingRequestAsyncById(BorrowingRequestId id);
    }
}
