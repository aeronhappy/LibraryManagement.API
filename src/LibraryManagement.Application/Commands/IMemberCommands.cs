using FluentResults;
using LibraryManagement.Application.Response;

namespace LibraryManagement.Application.Commands
{
    public interface IMemberCommands
    {
        Task<Result<MemberResponse>> CreateMemberAsync(string name, string email, CancellationToken cancellationToken);
        Task<Result> RemoveMemberAsync(Guid id, CancellationToken cancellationToken);
        Task<Result> UpdateMemberAsync(Guid id, string name, string email, CancellationToken cancellationToken);
        Task<Result> UpdateProfilePictureAsync(Guid id, string profilePic, CancellationToken cancellationToken);
    }
}
