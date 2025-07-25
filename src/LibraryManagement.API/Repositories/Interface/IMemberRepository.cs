using LibraryManagement.API.Datas.Models;

namespace LibraryManagement.API.Repositories.Interface
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllMemberesAsync();
        Task<Member?> GetMemberByIdAsync(Guid id);
        Task CreateMemberAsync(Member memberModel);
        Task RemoveMemberAsync(Guid id);
        Task UpdateMemberAsync(Guid id, string name, string email);
        Task AddBorrowedBookAsync(Guid id, Book borrowedBook);
        Task RemoveBorrowedBookAsync(Guid id, Book borrowedBook);
        Task SaveChangeAsync();
    }
}
