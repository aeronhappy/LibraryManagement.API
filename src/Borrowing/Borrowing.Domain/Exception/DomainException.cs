namespace Borrowing.Domain.Exception
{
    public class DomainException(string message) : RankException(message)
    {
    }
}