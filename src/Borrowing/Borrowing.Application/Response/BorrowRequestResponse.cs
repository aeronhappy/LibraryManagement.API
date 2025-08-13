using Borrowing.Domain.Entities;
using Borrowing.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borrowing.Application.Response
{
    public class BorrowRequestResponse
    {
        public Guid Id { get; private set; }
        public Member Borrower { get; private set; } = null!;
        public Book Book { get; private set; } = null!;
        public bool? IsAccepted { get; private set; }
    }
}
