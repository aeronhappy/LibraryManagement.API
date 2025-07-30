
namespace LibraryManagement.Domain.Exception
{
    public class MemberReachLimitException(string message) : DomainException(message)
    {
    }
}
