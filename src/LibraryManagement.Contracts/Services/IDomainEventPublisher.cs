using LibraryManagement.SharedKernel;

namespace LibraryManagement.Contracts.Services
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken);
    }
}
