using FluentResults;

namespace Borrowing.Application.Errors
{
    public class ConflictError(string message) : Error(message)
    {
    }

}
