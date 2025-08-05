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
        public List<BorrowingRecord> BorrowingHistory { get; set; } = [];




        public void MarkAsBorrowed()
        {
            if (IsBorrowed)
            {
                throw new BookUnavailableException("This book is already borrowed.");
            }
            IsBorrowed = true;

        }

        public void MarkAsReturned()
        {
            if (!IsBorrowed)
            {
                throw new BookAlreadyReturnedException("This book is not borrowed.");
            }
            IsBorrowed = false;
        }

    }
}
