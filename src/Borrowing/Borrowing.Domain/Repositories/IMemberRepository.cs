
using Borrowing.Domain.Entities;
using Borrowing.Domain.ValueObjects;

namespace Borrowing.Domain.Repositories
{
    public interface IMemberRepository
    {
        Task<Member?> GetMemberByIdAsync(MemberId id);
        Task CreateMemberAsync(Member memberModel);
        Task RemoveMemberAsync(MemberId id);
        Task UpdateMemberAsync(MemberId id, string? name, string? email);
    }
}
