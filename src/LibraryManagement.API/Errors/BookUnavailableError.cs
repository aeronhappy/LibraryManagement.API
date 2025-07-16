using FluentResults;

namespace LibraryManagement.API.Errors
{
    public class BookUnavailableError(string message) : Error(message)
    {
    }
}
