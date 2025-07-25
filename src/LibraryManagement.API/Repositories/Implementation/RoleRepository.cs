using AutoMapper.Execution;
using LibraryManagement.API.Datas;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repositories.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
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

        public async Task<Role?> GetRoleByIdAsync(Guid id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
