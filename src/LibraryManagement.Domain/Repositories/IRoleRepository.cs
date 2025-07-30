using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Repositories
{
    public interface IRoleRepository
    {

        Task<Role?> GetRoleByIdAsync(RoleId id);
        Task<List<Role>> GetAllRolesAsync();
        Task AddRoleAsync(Role role);
        Task SaveChangeAsync();
    }
}
