using LibraryManagement.API.Repositories;
using LibraryManagement.Domain.Repositories;

namespace LibraryManagement.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IBorrowingRecordRepository _borrowingRecordRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IRoleRepository _roleRepository;

        public UnitOfWork(ApplicationDbContext context, 
            IUserRepository userRepository,
            IMemberRepository memberRepository,
            IBorrowingRecordRepository borrowingRecordRepository,
            IBookRepository bookRepository,
            IRoleRepository roleRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _memberRepository = memberRepository;
            _borrowingRecordRepository = borrowingRecordRepository;
            _bookRepository = bookRepository;
            _roleRepository = roleRepository;
        }

        public IUserRepository Users => _userRepository;

        public IMemberRepository Members => _memberRepository;

        public IBookRepository Books => _bookRepository;

        public IBorrowingRecordRepository BorrowingRecords => _borrowingRecordRepository;

        public IRoleRepository Roles => _roleRepository;

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
