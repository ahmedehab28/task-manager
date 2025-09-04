using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    internal class CardListConfiguration : IEntityTypeConfiguration<CardList>
    {
        public void Configure(EntityTypeBuilder<CardList> builder)
        {
            builder.Property(l => l.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(l => l.Position)
                .IsRequired()
                .HasPrecision(7, 4);

            builder.HasOne(l => l.Board)
                .WithMany(b => b.Lists)
                .HasForeignKey(l => l.BoardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
