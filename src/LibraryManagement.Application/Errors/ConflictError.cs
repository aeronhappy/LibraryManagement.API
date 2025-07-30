using FluentResults;

namespace LibraryManagement.Application.Errors
{
    public class ConflictError(string message) : Error(message)
    {
    }

}
