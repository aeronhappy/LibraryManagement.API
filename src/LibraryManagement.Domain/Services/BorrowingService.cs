using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Services
{
    public class BorrowingService
    {
        public BorrowingRecord BorrowBook(Member member, Book book)
        {
            member.IncreaseBorrowedBookCount();
            book.MarkAsBorrowed();

            return BorrowingRecord.Create(
                member.Id,
                book.Id,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(10));   //10 days


        }

        public void ProcessReturn(BorrowingRecord record)
        {
            record.Process();
        }
    }
}
