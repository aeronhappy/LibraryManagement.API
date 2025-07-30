using FluentResults;

namespace LibraryManagement.Application.Errors
{
    public class EntityNotFoundError(string message) : Error(message)
    {

    }
}
