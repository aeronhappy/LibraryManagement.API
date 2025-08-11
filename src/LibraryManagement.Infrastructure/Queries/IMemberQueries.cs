using FluentResults;
using LibraryManagement.Application.Response;

namespace LibraryManagement.Infrastructure.Queries
{
    public interface IMemberQueries
    {
        Task<List<MemberResponse>> GetAllMemberesAsync(string searchText);
        Task<MemberResponse?> GetMemberByIdAsync(Guid id);
    }
}
