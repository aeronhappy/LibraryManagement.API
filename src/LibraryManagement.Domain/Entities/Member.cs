
using LibraryManagement.Domain.Exception;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Entities
{
    public class Member
    {
        public required MemberId Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public int MaxBooksAllowed { get; set; }
        public int BorrowedBooksCount { get; set; }
        public List<Book> Books { get; set; } = [];

        public void IncreaseBorrowedBookCount(BorrowingRecord borrowingRecord)
        {
            if (BorrowedBooksCount >= MaxBooksAllowed)
            {
                throw new MemberReachLimitException($"Cant borrow more than {MaxBooksAllowed}");
            }
            BorrowedBooksCount++;
            Books.Add(borrowingRecord.Book);
        }

        public void DecreaseBorrowedBookCount(BorrowingRecord borrowingRecord)
        {
            if (BorrowedBooksCount <= 0)
            {
                throw new MemberCantReturnWithoutBorrowingException("Member cant return without borrowing books");
            }
            BorrowedBooksCount--;
            Books.Remove(borrowingRecord.Book);
        }
    }
}
