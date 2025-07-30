
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

        public void IncreaseBorrowedBookCount()
        {
            if (BorrowedBooksCount >= MaxBooksAllowed)
            {
                throw new MemberReachLimitException($"Cant borrow more than {MaxBooksAllowed}");
            }
            BorrowedBooksCount++;
        }

        public void DecreaseBorrowedBookCount()
        {
            if (BorrowedBooksCount <= 0)
            {
                throw new MemberCantReturnWithoutBorrowingException("Member cant return without borrowing books");
            }
            BorrowedBooksCount--;
        }
    }
}
