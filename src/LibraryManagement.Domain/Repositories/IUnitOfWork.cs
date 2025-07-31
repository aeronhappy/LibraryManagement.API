namespace LibraryManagement.Domain.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IMemberRepository Members { get; }
        IBookRepository Books { get; }
        IBorrowingRecordRepository BorrowingRecords { get; }
        IRoleRepository Roles { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
