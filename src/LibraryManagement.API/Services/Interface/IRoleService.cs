using FluentResults;
using LibraryManagement.API.DTOs.Response;

namespace LibraryManagement.API.Services.Interface
{
    public interface IRoleService
    {
        Task<List<RoleResponse>> GetAllRoleAsync();
        Task<RoleResponse?> GetRoleByIdAsync(Guid id);
        Task<Result<RoleResponse>> CreateRoleAsync(String name);
    }
}
