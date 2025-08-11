using AutoMapper;
using AutoMapper.QueryableExtensions;
using Identity.Application.Queries;
using Identity.Application.Response;
using Identity.Domain.ValueObjects;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.QueryHandler
{
    public class RoleQueries : IRoleQueries
    {
        private readonly IdentityDbContext _context;
        private readonly IMapper _mapper;

        public RoleQueries(IdentityDbContext context, IMapper mapper)
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
