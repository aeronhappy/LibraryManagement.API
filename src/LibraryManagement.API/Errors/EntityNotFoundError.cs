using FluentResults;

namespace LibraryManagement.API.Errors
{
    public class EntityNotFoundError(string message) : Error(message)
    {

    }
}
