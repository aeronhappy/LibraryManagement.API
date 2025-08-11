
using FluentResults;
using LibraryManagement.Application.Response;

namespace LibraryManagement.Infrastructure.Queries
{
    public interface IRoleQueries
    {
        Task<List<RoleResponse>> GetAllRoleAsync();
        Task<RoleResponse?> GetRoleByIdAsync(Guid id);
    }
}
