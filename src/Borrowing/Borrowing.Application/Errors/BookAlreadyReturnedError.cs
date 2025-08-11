using FluentResults;

namespace Borrowing.Application.Errors
{
    public class BookAlreadyReturnedError(string message) : Error(message)
    {
    }
}
