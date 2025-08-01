using LibraryManagement.Domain.Exception;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Entities
{
    public class Book
    {
        public required BookId Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ISBN { get; set; }
        public bool IsBorrowed { get; set; }
        public DateTime? DateBorrowed { get; set; }
        public MemberId? BorrowerId { get; set; }
        public Member? Borrower { get; set; }
        public DateTime? DateOverdue { get; set; }




        public void MarkAsBorrowed(BorrowingRecord borrowingRecord)
        {
            if (IsBorrowed)
            {
                throw new BookUnavailableException("This book is already borrowed.");
            }
            IsBorrowed = true;
            DateBorrowed = DateTime.UtcNow;
            BorrowerId = borrowingRecord.BorrowerId;
            DateOverdue= borrowingRecord.DateOverdue;

        }

        public void MarkAsReturned()
        {
            if (!IsBorrowed)
            {
                throw new BookAlreadyReturnedException("This book is not borrowed.");
            }
            IsBorrowed = false;
            DateBorrowed = null;
            BorrowerId = null;
            DateOverdue = null;
        }

    }
}
