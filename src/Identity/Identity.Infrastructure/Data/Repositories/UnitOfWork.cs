using Identity.Domain.Repositories;

namespace Identity.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UnitOfWork(IdentityDbContext context,
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _context = context;
            _userRepository = userRepository;
          
            _roleRepository = roleRepository;
        }

        public IUserRepository Users => _userRepository;
        public IRoleRepository Roles => _roleRepository;

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
