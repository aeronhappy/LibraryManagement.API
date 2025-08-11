using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;

namespace Identity.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(UserId id);
        Task<List<User>> GetUsersAsync();
    }
}
