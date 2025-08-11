using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.QueryHandler
{
    public class RoleQueries : IRoleQueries
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoleQueries(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<List<RoleResponse>> GetAllRoleAsync()
        {
            return await _context.Roles.ProjectTo<RoleResponse>(_mapper.ConfigurationProvider).ToListAsync();
            
        }

        public async Task<RoleResponse?> GetRoleByIdAsync(Guid id)
        {
            var roleResponse = await _context.Roles
                .Where(r => r.Id == new RoleId(id))
                .ProjectTo<RoleResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return roleResponse;
        }
    }
}
