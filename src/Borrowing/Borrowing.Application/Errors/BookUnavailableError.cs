using FluentResults;

namespace Borrowing.Application.Errors
{
    public class BookUnavailableError(string message) : Error(message)
    {
    }
}
