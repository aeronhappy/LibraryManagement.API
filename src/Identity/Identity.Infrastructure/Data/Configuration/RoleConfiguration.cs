using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> role)
        {
            role.HasKey(u => u.Id);
            role.Property(u => u.Id)
                .HasConversion(u => u.Value, value => new RoleId(value));

        }
    }
}
