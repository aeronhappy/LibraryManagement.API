using FluentResults;

namespace LibraryManagement.Application.Errors
{
    public class UnableToDeleteError(string message) : Error(message)
    {
    }
}
