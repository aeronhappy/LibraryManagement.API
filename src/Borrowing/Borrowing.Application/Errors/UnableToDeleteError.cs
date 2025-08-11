using FluentResults;

namespace Borrowing.Application.Errors
{
    public class UnableToDeleteError(string message) : Error(message)
    {
    }
}
