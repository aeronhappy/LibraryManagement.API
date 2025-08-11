using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;

namespace Identity.Domain.Repositories
{
    public interface IRoleRepository
    {

        Task<Role?> GetRoleByIdAsync(RoleId id);
        Task<List<Role>> GetAllRolesAsync();
        Task AddRoleAsync(Role role);

    }
}
