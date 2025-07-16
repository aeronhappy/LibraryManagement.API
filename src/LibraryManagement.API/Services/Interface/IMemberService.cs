using FluentResults;
using LibraryManagement.API.DTOs.Response;

namespace LibraryManagement.API.Services.Interface
{
    public interface IMemberService
    {
        Task<List<MemberResponse>> GetAllMemberesAsync(string searchText);
        Task<MemberResponse?> GetMemberByIdAsync(Guid id);
        Task<Result<MemberResponse>> CreateMemberAsync(string name, string email);
        Task<Result> RemoveMemberAsync(Guid id);
        Task<Result> UpdateMemberAsync(Guid id, string name, string email);
    }
}
