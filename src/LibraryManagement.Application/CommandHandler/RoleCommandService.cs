using AutoMapper;
using FluentResults;
using LibraryManagement.API.Services.Interface;
using LibraryManagement.Application.Errors;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Application.CommandHandler
{
    public class RoleCommandService : IRoleCommandService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleCommandService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<Result<RoleResponse>> CreateRoleAsync(string name)
        {

            var roles = await _roleRepository.GetAllRolesAsync();

            if (roles.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                return Result.Fail(new ConflictError($"The role '{name}' already exists in the database."));
            }

            var newRole = new Role
            {
                Id = new RoleId(Guid.NewGuid()) ,
                Name = name
            };

            await _roleRepository.AddRoleAsync(newRole);
            await _roleRepository.SaveChangeAsync();

            var response = _mapper.Map<RoleResponse>(newRole); ;

            return Result.Ok(response);
        }

        public async Task<List<RoleResponse>> GetAllRoleAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return roles.Select(r => _mapper.Map<RoleResponse>(r)).ToList();
        }

        public async Task<RoleResponse?> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(new RoleId(id));
            return _mapper.Map<RoleResponse>(role);
        }
    }
}
