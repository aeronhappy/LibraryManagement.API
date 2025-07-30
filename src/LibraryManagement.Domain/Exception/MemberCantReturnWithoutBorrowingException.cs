
namespace LibraryManagement.Domain.Exception
{
    public class MemberCantReturnWithoutBorrowingException(string message) : DomainException(message)
    {
    }
}
