using Borrowing.Domain.Entities;
using Borrowing.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borrowing.Infrastructure.Data.Configuration
{
    public class BorrowingRequestConfiguration : IEntityTypeConfiguration<BorrowingRequest>
    {
        public void Configure(EntityTypeBuilder<BorrowingRequest> borrowingRequest)
        {
            borrowingRequest.HasKey(u => u.Id);
            borrowingRequest.Property(u => u.Id)
                .HasConversion(u => u.Value, value => new BorrowingRequestId(value));
            borrowingRequest.Property(u => u.BookId)
               .HasConversion(u => u.Value, value => new BookId(value));
            borrowingRequest.Property(u => u.BorrowerId)
                .HasConversion(u => u.Value, value => new MemberId(value));
        }


    }
}
