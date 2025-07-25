using LibraryManagement.API.Datas.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Datas
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
             .HasMany(u => u.Roles)
             .WithMany(r => r.Users);
            base.OnModelCreating(modelBuilder);
         
        }
        public DbSet<Member> Members { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
