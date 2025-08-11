using FluentResults;

namespace Borrowing.Application.Errors
{
    public class MemberCantReturnWithoutBorrowingError(string message) : Error(message)
    {
    }
}
