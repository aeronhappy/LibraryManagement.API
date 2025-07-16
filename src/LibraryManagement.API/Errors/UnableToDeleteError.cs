using FluentResults;

namespace LibraryManagement.API.Errors
{
    public class UnableToDeleteError(string message) : Error(message)
    {
    }
}
