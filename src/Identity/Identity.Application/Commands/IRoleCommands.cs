
using FluentResults;
using Identity.Application.Response;

namespace Identity.Application.Commands
{
    public interface IRoleCommands
    {
        Task<Result<RoleResponse>> CreateRoleAsync(string name, CancellationToken cancellationToken);
    }
}
