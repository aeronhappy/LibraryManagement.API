using LibraryManagement.Domain.Exception;
using LibraryManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class BorrowingRequest
    {
        public BorrowingRequestId Id { get; private set; }
        public MemberId BorrowerId { get; private set; }
        public Member Borrower { get; private set; } = null!;
        public BookId BookId { get; private set; }
        public Book Book { get; private set; } = null!;
        public bool? IsAccepted { get; private set; }


        private BorrowingRequest(
          BorrowingRequestId id,
          MemberId borrowerId,
          BookId bookId,
          bool? isAccepted = null)
        {
            Id = id;
            BorrowerId = borrowerId;
            BookId = bookId;
            IsAccepted = isAccepted;
        }

        public static BorrowingRequest Create(
          MemberId memberId,
          BookId bookId)
        {
            return new BorrowingRequest(
                new BorrowingRequestId(Guid.NewGuid()),
                memberId,
                bookId,
                null);
        }

        public void Accept()
        {
            IsAccepted = true;
        }

    }
}
