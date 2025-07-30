
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Repositories
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllMemberesAsync();
        Task<Member?> GetMemberByIdAsync(MemberId id);
        Task CreateMemberAsync(Member memberModel);
        Task RemoveMemberAsync(MemberId id);
        Task UpdateMemberAsync(MemberId id, string name, string email);
        //Task AddBorrowedBookAsync(Guid id, Book borrowedBook);
        //Task RemoveBorrowedBookAsync(Guid id, Book borrowedBook);
        //Task SaveChangeAsync();
    }
}
