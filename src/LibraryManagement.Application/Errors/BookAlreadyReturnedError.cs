using FluentResults;

namespace LibraryManagement.Application.Errors
{
    public class BookAlreadyReturnedError(string message) : Error(message)
    {
    }
}
