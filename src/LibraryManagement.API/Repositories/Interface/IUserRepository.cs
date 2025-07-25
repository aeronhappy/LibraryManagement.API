using LibraryManagement.API.Datas.Models;

namespace LibraryManagement.API.Repositories.Interface
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<List<User>> GetUsersAsync();
        Task SaveChangeAsync();
    }
}
