using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configuration
{
    public class BorrowingRecordConfiguration : IEntityTypeConfiguration<BorrowingRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowingRecord> borrowingRecord)
        {
            borrowingRecord.HasKey(u => u.Id);
            borrowingRecord.Property(u => u.Id)
                .HasConversion(u => u.Value, value => new BorrowingRecordId(value));
            borrowingRecord.Property(u => u.BookId)
               .HasConversion(u => u.Value, value => new BookId(value)); 
            borrowingRecord.Property(u => u.BorrowerId)
                .HasConversion(u => u.Value, value => new MemberId(value));
        }
    }
}
