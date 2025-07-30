using FluentResults;

namespace LibraryManagement.Application.Errors
{
    public class MemberReachLimitError(string message) : Error(message)
    {
    }
}
