using AutoMapper;
using FluentResults;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.API.Errors;
using LibraryManagement.API.Repositories.Interface;
using LibraryManagement.API.Services.Interface;

namespace LibraryManagement.API.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
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
                Id = Guid.NewGuid(),
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
            var role = await _roleRepository.GetRoleByIdAsync(id);
            return _mapper.Map<RoleResponse>(role);
        }
    }
}
