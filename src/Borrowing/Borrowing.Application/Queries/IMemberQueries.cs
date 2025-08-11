using Borrowing.Application.Response;
using FluentResults;

namespace Borrowing.Application.Queries
{
    public interface IMemberQueries
    {
        Task<List<MemberResponse>> GetAllMemberesAsync(string searchText);
        Task<MemberResponse?> GetMemberByIdAsync(Guid id);
    }
}
