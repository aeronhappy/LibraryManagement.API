using FluentResults;
using LibraryManagement.Application.Response;

namespace LibraryManagement.API.Services.Interface
{
    public interface IMemberCommandService
    {
        Task<List<MemberResponse>> GetAllMemberesAsync(string searchText);
        Task<MemberResponse?> GetMemberByIdAsync(Guid id);
        Task<Result<MemberResponse>> CreateMemberAsync(string name, string email);
        Task<Result> RemoveMemberAsync(Guid id);
        Task<Result> UpdateMemberAsync(Guid id, string name, string email);
    }
}
