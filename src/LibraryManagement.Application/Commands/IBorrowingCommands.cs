using FluentResults;

namespace LibraryManagement.Application.Commands
{
    public interface IBorrowingCommands
    {
        Task<Result> BorrowAsync(Guid bookId, Guid memberId, CancellationToken cancellationToken);
        Task<Result> ReturnAsync(Guid bookId, Guid memberId, CancellationToken cancellationToken);
        Task<Result> BorrowRequestAsync(Guid bookId, Guid memberId, CancellationToken cancellationToken);
        Task<Result> AcceptRequestAsync(Guid requestBorrowId, CancellationToken cancellationToken);
    }
}
