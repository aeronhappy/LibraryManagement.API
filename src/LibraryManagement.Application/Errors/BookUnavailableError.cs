using FluentResults;

namespace LibraryManagement.Application.Errors
{
    public class BookUnavailableError(string message) : Error(message)
    {
    }
}
