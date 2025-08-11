
using Borrowing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Borrowing.Infrastructure.Data
{
    public class BorrowingDbContext : DbContext
    {
        public BorrowingDbContext(DbContextOptions<BorrowingDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Borrowing");
            modelBuilder
              .ApplyConfigurationsFromAssembly(typeof(BorrowingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Member> Members { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowingRecord> BorrowingRecords { get; set; }
        public DbSet<BorrowingRequest> BorrowingRequests { get; set; }
    }
}
