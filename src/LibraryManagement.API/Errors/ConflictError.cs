using FluentResults;

namespace LibraryManagement.API.Errors
{
    public class ConflictError(string message) : Error(message)
    {
    }

}
