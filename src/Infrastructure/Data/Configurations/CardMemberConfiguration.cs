using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CardMemberConfiguration : IEntityTypeConfiguration<CardMember>
    {
        public void Configure(EntityTypeBuilder<CardMember> builder)
        {
            builder.HasKey(cm => new { cm.CardId, cm.UserId });

            builder.Property(cm => cm.JoinedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(cm => cm.Card)
                .WithMany(c => c.Members)
                .HasForeignKey(cm => cm.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Helpful index for queries
            builder.HasIndex(cm => cm.UserId);
            builder.HasIndex(cm => cm.CardId);
        }
    }
}
