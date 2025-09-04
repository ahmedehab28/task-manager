
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    internal class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.HasKey(pm => new { pm.ProjectId, pm.UserId });

            builder.Property(pm => pm.Role).IsRequired();

            builder.Property(pm => pm.JoinedAt)
             .IsRequired()
             .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(pm => pm.Project)
             .WithMany(p => p.Members)
             .HasForeignKey(pm => pm.ProjectId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
             .WithMany()
             .HasForeignKey(pm => pm.UserId)
             .OnDelete(DeleteBehavior.Restrict);   // often safer than cascade for user deletes (Look into this)

            // Helpful index for queries
            builder.HasIndex(pm => pm.UserId);
            builder.HasIndex(pm => pm.ProjectId);

        }
    }
}
