using Borrowing.Domain.Entities;
using Borrowing.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borrowing.Infrastructure.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> book)
        {
            book.HasKey(b => b.Id);
            book.Property(b => b.Id)
                .HasConversion(b => b.Value, value => new BookId(value));

        }
    }
}
