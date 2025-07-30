using FluentResults;

namespace LibraryManagement.Application.Commands
{
    public interface IBorrowingCommandService
    {
        Task<Result> BorrowAsync(Guid bookId, Guid memberId, CancellationToken cancellationToken);
        Task<Result> ReturnAsync(Guid bookId, Guid memberId, CancellationToken cancellationToken);
    }
}
