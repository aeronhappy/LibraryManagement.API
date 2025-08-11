
using FluentResults;
using LibraryManagement.Application.Response;

namespace LibraryManagement.Application.Commands
{
    public interface IRoleCommands
    {
        Task<Result<RoleResponse>> CreateRoleAsync(string name, CancellationToken cancellationToken);
    }
}
