using FluentResults;

namespace LibraryManagement.Application.Errors
{
    public class MemberCantReturnWithoutBorrowingError(string message) : Error(message)
    {
    }
}
