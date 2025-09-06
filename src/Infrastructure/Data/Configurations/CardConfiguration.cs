
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Description)
                .HasMaxLength(512);

            builder.Property(c => c.Position)
               .IsRequired()
               .HasPrecision(18, 4);

            builder.HasOne(c => c.CardList)
                .WithMany(l => l.Cards)
                .HasForeignKey(c => c.CardListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Members)
                .WithOne(cm => cm.Card)
                .HasForeignKey(cm => cm.CardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
