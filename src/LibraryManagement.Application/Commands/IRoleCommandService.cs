
using FluentResults;
using LibraryManagement.Application.Response;

namespace LibraryManagement.Application.Commands
{
    public interface IRoleCommandService
    {
        Task<Result<RoleResponse>> CreateRoleAsync(string name, CancellationToken cancellationToken);
    }
}
