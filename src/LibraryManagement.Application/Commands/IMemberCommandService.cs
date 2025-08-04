using FluentResults;
using LibraryManagement.Application.Response;

namespace LibraryManagement.Application.Commands
{
    public interface IMemberCommandService
    {
        Task<Result<MemberResponse>> CreateMemberAsync(string name, string email, CancellationToken cancellationToken);
        Task<Result> RemoveMemberAsync(Guid id, CancellationToken cancellationToken);
        Task<Result> UpdateMemberAsync(Guid id, string name, string email, CancellationToken cancellationToken);
    }
}
