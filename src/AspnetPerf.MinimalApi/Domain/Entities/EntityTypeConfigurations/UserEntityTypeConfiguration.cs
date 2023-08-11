using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspnetPerf.MinimalApi.Domain.Entities.EntityTypeConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Email).HasMaxLength(256);
        builder.Property(p => p.FirstName).HasMaxLength(128);
        builder.Property(p => p.LastName).HasMaxLength(128);
        builder.Property(p => p.PasswordHash).HasMaxLength(256);
    }
}
