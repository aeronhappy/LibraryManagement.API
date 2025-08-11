using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder
              .ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
