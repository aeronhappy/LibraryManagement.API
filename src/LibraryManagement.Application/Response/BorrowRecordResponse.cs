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
        public Guid Id { get; set; }
        public BookBorrowedResponse Book { get; set; } = null!;
        public BorrowerResponse Borrower { get; set; } = null!;
        public DateTimeOffset DateBorrowed { get;  set; }
        public DateTimeOffset DateOverdue { get;  set; }
        public DateTimeOffset? DateReturned { get;  set; }
    }


    public class MemberBorrowRecordResponse
    {
        public Guid Id { get; private set; }
        public BorrowerResponse Borrower { get;  set; } = null!;
        public DateTimeOffset DateBorrowed { get;  set; }
        public DateTimeOffset DateOverdue { get;  set; }
        public DateTimeOffset? DateReturned { get;  set; }
    }

    public class BookBorrowRecordResponse
    {
        public Guid Id { get; private set; }
        public BookBorrowedResponse Book { get;  set; } = null!;
        public DateTimeOffset DateBorrowed { get;  set; }
        public DateTimeOffset DateOverdue { get;  set; }
        public DateTimeOffset? DateReturned { get;  set; }
    }
}
