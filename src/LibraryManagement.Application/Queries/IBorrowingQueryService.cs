using FluentResults;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Commands
{
    public interface IBorrowingQueryService
    {
        Task<List<BorrowingRecord>> GetAllBorrowingRecord(string searchText, DateTime? dataTime);
        Task<BorrowingRecord?> GetLatestBorrowedRecordByBookId(Guid bookId);
    }
}
