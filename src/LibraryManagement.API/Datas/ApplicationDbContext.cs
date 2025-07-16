using LibraryManagement.API.Datas.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Datas
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Member> Members { get; set; }

        public DbSet<Book> Books { get; set; }
    }
}
