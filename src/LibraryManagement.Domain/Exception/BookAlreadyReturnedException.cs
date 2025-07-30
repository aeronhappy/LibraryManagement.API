namespace LibraryManagement.Domain.Exception
{
    public class BookAlreadyReturnedException(string message) : DomainException(message)
    {
    }
}
