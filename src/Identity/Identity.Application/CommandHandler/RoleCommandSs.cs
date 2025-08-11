using AutoMapper;
using FluentResults;
using Identity.Application.Commands;
using Identity.Application.Errors;
using Identity.Application.Response;
using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Identity.Domain.ValueObjects;

namespace Identity.Application.CommandHandler
{
    public class RoleCommandSs : IRoleCommands
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleCommandSs(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<RoleResponse>> CreateRoleAsync(string name, CancellationToken cancellationToken)
        {

            var roles = await _unitOfWork.Roles.GetAllRolesAsync();

            if (roles.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                return Result.Fail(new ConflictError($"The role '{name}' already exists in the database."));
            }

            var newRole = new Role
            {
                Id = new RoleId(Guid.NewGuid()),
                Name = name
            };

            await _unitOfWork.Roles.AddRoleAsync(newRole);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<RoleResponse>(newRole); ;

            return Result.Ok(response);
        }

       
    }
}
