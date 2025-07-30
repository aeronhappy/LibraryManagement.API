namespace LibraryManagement.Domain.Exception
{
    public class BookUnavailableException(string message) : DomainException(message)
    {
    }
}
