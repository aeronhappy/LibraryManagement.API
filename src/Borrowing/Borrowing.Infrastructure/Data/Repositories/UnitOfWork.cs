using Borrowing.Domain.Repositories;

namespace Borrowing.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BorrowingDbContext _context;
        private readonly IMemberRepository _memberRepository;
        private readonly IBorrowingRecordRepository _borrowingRecordRepository;
        private readonly IBookRepository _bookRepository;

        public UnitOfWork(BorrowingDbContext context,
            IMemberRepository memberRepository,
            IBorrowingRecordRepository borrowingRecordRepository,
            IBookRepository bookRepository)
        {
            _context = context;
            _memberRepository = memberRepository;
            _borrowingRecordRepository = borrowingRecordRepository;
            _bookRepository = bookRepository;
        }



        public IMemberRepository Members => _memberRepository;

        public IBookRepository Books => _bookRepository;

        public IBorrowingRecordRepository BorrowingRecords => _borrowingRecordRepository;



        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
