using LibraryManagement.API.Datas;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(v => v.Roles)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
         .Include(v => v.Roles)
         .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.Include(v => v.Roles).ToListAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
