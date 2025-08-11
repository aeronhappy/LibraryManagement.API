using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user.HasKey(u => u.Id);
            user.Property(u => u.Id)
                .HasConversion(u => u.Value, value => new UserId(value));

        }
    }
}
