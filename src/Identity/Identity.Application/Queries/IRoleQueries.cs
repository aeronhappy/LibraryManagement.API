using Identity.Application.Response;

namespace Identity.Application.Queries
{
    public interface IRoleQueries
    {
        Task<List<RoleResponse>> GetAllRoleAsync();
        Task<RoleResponse?> GetRoleByIdAsync(Guid id);
    }
}
