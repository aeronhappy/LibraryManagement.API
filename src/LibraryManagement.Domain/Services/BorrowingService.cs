using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Services
{
    public class BorrowingService
    {
        public BorrowingRecord BorrowBook(Member member, Book book)
        {

            var borrowingRecord = BorrowingRecord.Create(
                member.Id,
                book.Id,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(10));

            member.IncreaseBorrowedBookCount(borrowingRecord);
            book.MarkAsBorrowed(borrowingRecord);

            return borrowingRecord; //10 days


        }

        public void ProcessReturn(BorrowingRecord record)
        {
            record.Borrower.IncreaseBorrowedBookCount(record);
            record.Book.MarkAsReturned();
            record.Process();
        }

       
    }
}
