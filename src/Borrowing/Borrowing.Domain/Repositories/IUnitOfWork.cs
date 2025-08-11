namespace Borrowing.Domain.Repositories
{
    public interface IUnitOfWork
    {
        IMemberRepository Members { get; }
        IBookRepository Books { get; }
        IBorrowingRecordRepository BorrowingRecords { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
