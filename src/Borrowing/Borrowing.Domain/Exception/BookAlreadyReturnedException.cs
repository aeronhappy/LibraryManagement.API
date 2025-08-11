namespace Borrowing.Domain.Exception
{
    public class BookAlreadyReturnedException(string message) : DomainException(message)
    {
    }
}
