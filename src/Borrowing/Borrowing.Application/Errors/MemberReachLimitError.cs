using FluentResults;

namespace Borrowing.Application.Errors
{
    public class MemberReachLimitError(string message) : Error(message)
    {
    }
}
