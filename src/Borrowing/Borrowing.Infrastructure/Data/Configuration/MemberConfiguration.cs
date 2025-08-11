using Borrowing.Domain.Entities;
using Borrowing.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borrowing.Infrastructure.Data.Configuration
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> member)
        {
            member.HasKey(u => u.Id);
            member.Property(u => u.Id)
                .HasConversion(u => u.Value, value => new MemberId(value));
        }
    }
}
