using FluentResults;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Errors;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exception;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.Services;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Application.CommandHandler
{
    public class BorrowingQueryService : IBorrowingQueryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowingQueryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BorrowingRecord>> GetAllBorrowingRecord(string searchText, DateTime? dataTime)
        {
          var allRecord = await _unitOfWork.BorrowingRecords.GetAllBorrowingRecord();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                allRecord = allRecord
                    .Where(record => 
                    record.Book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase )|| 
                    record.Borrower.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (dataTime.HasValue)
            {
                allRecord = allRecord
                    .Where(record => 
                    record.DateBorrowed.Date == dataTime.Value.Date)
                    .ToList();
            }

            return allRecord;
        }

        public async Task<BorrowingRecord?> GetLatestBorrowedRecordByBookId(Guid bookId)
        {
          return  await _unitOfWork.BorrowingRecords.GetLatestBorrowedRecordByBookId(new BookId(bookId));
        }
    }
}
