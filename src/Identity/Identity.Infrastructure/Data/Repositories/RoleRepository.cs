using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IdentityDbContext _context;

        public RoleRepository(IdentityDbContext context)
        {
            _context = context;
        }
        public async Task AddRoleAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(RoleId id)
        {
            return await _context.Roles.FindAsync(id);
        }



    }
}
