
using FluentResults;
using LibraryManagement.Application.Response;

namespace LibraryManagement.API.Services.Interface
{
    public interface IRoleCommandService
    {
        Task<List<RoleResponse>> GetAllRoleAsync();
        Task<RoleResponse?> GetRoleByIdAsync(Guid id);
        Task<Result<RoleResponse>> CreateRoleAsync(String name);
    }
}
