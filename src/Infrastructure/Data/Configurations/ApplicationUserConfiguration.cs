using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.UserName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.AvatarUrl)
                .HasMaxLength(500);

            builder.Property(u => u.TimeZone)
                .HasMaxLength(100);

            builder.Property(u => u.Locale)
                .HasMaxLength(20);

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.UserName)
                .IsUnique();
        }
    }
}
