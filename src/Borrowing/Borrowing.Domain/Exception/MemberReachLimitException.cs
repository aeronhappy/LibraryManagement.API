namespace Borrowing.Domain.Exception
{
    public class MemberReachLimitException(string message) : DomainException(message)
    {
    }
}
