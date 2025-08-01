using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> book)
        {
            book.HasKey(u => u.Id);
            book.Property(u => u.Id)
                .HasConversion(u => u.Value, value => new BookId(value));
            book.Property(u => u.BorrowerId)
                .HasConversion(u => u.Value, value => new MemberId(value));
        }
    }
}
