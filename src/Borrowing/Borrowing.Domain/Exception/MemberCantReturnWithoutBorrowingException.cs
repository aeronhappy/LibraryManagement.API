namespace Borrowing.Domain.Exception
{
    public class MemberCantReturnWithoutBorrowingException(string message) : DomainException(message)
    {
    }
}
