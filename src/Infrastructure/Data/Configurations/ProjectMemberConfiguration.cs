
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

            builder.HasOne(pm => pm.User)
             .WithMany(u => u.ProjectMemberships)
             .HasForeignKey(pm => pm.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
