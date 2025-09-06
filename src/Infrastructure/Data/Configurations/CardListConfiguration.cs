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

            builder.Property(c => c.Position)
               .IsRequired()
               .HasPrecision(18, 4);


            builder.HasOne(cl => cl.Board)
                .WithMany(b => b.CardLists)
                .HasForeignKey(cl => cl.BoardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
