using FluentResults;

namespace Identity.Application.Errors
{
    public class ConflictError(string message) : Error(message)
    {
    }

}
