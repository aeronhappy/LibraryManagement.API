using FluentResults;

namespace LibraryManagement.API.Errors
{
    public class MemberReachLimitError(string message):Error(message)
    {
    }
}
