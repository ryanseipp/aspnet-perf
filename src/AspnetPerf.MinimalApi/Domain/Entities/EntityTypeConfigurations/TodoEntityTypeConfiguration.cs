using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspnetPerf.MinimalApi.Domain.Entities.EntityTypeConfigurations;

public class TodoEntityTypeConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Text).HasMaxLength(512);
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(16);

        builder
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
