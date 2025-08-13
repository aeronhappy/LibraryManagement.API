using Borrowing.Application.Response;
using Borrowing.Domain.Entities;

namespace Borrowing.Application.Queries
{
    public interface IBorrowingQueries
    {
        Task<List<BorrowRecordResponse>> GetAllBorrowRecordsAsync(string searchText, DateTime? dataTime);
        Task<List<BorrowRecordResponse>> GetUnreturnedBorrowRecordsAsync(string searchText, DateTime? dataTime);
        Task<List<BorrowRecordResponse>> GetReturnedBorrowRecordsAsync(string searchText, DateTime? dataTime);
        Task<BorrowRecordResponse?> GetBorrowingRecordById(Guid recordId);

        ///
        Task<List<BorrowRequestResponse>> GetBorrowRequestAsync(string searchText);
    }
}
