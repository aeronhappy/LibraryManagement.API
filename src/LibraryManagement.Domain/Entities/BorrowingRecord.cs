using LibraryManagement.Domain.Exception;
using LibraryManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class BorrowingRecord
    {

        public BorrowingRecordId Id { get; private set; }
        public MemberId BorrowerId { get; private set; }
        public Member Borrower { get; private set; } = null!;
        public BookId BookId { get; private set; }
        public Book Book { get; private set; } = null!;
        public DateTime DateBorrowed { get; private set; }
        public DateTime DateOverdue { get; private set; }
        public DateTime? DateReturned { get; private set; }


        private BorrowingRecord(
            BorrowingRecordId id,
            MemberId borrowerId,
            BookId bookId,
            DateTime dateBorrowed,
            DateTime dateOverdue,
            DateTime? dateReturned)
        {
            Id = id;
            BorrowerId = borrowerId;
            BookId = bookId;
            DateBorrowed = dateBorrowed;
            DateOverdue = dateOverdue;
            DateReturned = dateReturned;
        }

        public static BorrowingRecord Create(
            MemberId memberId,
            BookId bookId,
            DateTime dateBorrowed,
            DateTime dateOverdue)
        {
            return new BorrowingRecord(
                new BorrowingRecordId(Guid.NewGuid()),
                memberId,
                bookId,
                dateBorrowed,
                dateOverdue,
                null);
        }

        public void Process()
        {
            if (DateReturned.HasValue)
                throw new BookAlreadyReturnedException("Can't return not borrowed Book");
            DateReturned = DateTime.UtcNow;
          
        }

    }
}
