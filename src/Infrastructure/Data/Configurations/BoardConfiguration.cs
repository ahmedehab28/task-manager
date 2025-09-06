
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.Description)
                .HasMaxLength(512);

            builder.Property(b => b.BoardType)
                .IsRequired();

            builder.HasMany(b => b.CardLists)
                .WithOne(cl => cl.Board)
                .HasForeignKey(cl => cl.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
