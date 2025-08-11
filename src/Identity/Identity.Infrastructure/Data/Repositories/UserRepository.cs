using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;

        public UserRepository(IdentityDbContext context)
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

        public async Task<User?> GetByIdAsync(UserId id)
        {
            return await _context.Users
         .Include(v => v.Roles)
         .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.Include(v => v.Roles).ToListAsync();
        }


    }
}
