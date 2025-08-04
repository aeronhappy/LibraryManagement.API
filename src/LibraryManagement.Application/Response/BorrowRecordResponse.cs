using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Response
{
    
    public class BorrowRecordResponse
    {
        public  BorrowingRecordId Id { get; private set; }
        public MemberId BorrowerId { get; private set; }
        public Member Borrower { get; private set; } = null!;
        public BookId BookId { get; private set; }
        public Book Book { get; private set; } = null!;
        public DateTime DateBorrowed { get; private set; }
        public DateTime DateOverdue { get; private set; }
        public DateTime? DateReturned { get; private set; }
    }
}
