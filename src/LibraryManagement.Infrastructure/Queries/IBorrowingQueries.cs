using FluentResults;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Infrastructure.Queries
{
    public interface IBorrowingQueries
    {
        Task<List<BorrowRecordResponse>> GetAllBorrowRecordsAsync(string searchText, DateTime? dataTime);
        Task<List<BorrowRecordResponse>> GetUnreturnedBorrowRecordsAsync(string searchText, DateTime? dataTime);
        Task<List<BorrowRecordResponse>> GetReturnedBorrowRecordsAsync(string searchText, DateTime? dataTime);

        Task<BorrowRecordResponse?> GetBorrowingRecordById(Guid recordId);
    }
}
