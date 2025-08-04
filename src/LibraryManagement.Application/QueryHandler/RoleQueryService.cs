using AutoMapper;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Application.CommandHandler
{
    public class RoleQueryService : IRoleQueryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleQueryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

     

        public async Task<List<RoleResponse>> GetAllRoleAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllRolesAsync();
            return roles.Select(r => _mapper.Map<RoleResponse>(r)).ToList();
        }

        public async Task<RoleResponse?> GetRoleByIdAsync(Guid id)
        {
            var role = await _unitOfWork.Roles.GetRoleByIdAsync(new RoleId(id));
            return _mapper.Map<RoleResponse>(role);
        }
    }
}
