using FluentResults;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Infrastructure.Queries
{
    public interface IBorrowingQueryService
    {
        Task<List<BorrowRecordResponse>> GetAllBorrowingRecord(string searchText, DateTime? dataTime);

        Task<BorrowRecordResponse?> GetBorrowingRecordById(Guid recordId);
    }
}
