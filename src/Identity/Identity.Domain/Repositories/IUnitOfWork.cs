namespace Identity.Domain.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
