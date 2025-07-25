using LibraryManagement.API.Datas.Models;

namespace LibraryManagement.API.Repositories.Interface
{
    public interface IRoleRepository
    {

        Task<Role?> GetRoleByIdAsync(Guid id);
        Task<List<Role>> GetAllRolesAsync();
        Task AddRoleAsync(Role role);
        Task SaveChangeAsync();
    }
}
