using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> Users { get; }
        DbSet<Project> Projects { get; }
        DbSet<ProjectMember> ProjectMembers { get; }
        DbSet<Board> Boards { get; }
        DbSet<CardList> CardLists { get; }
        DbSet<Card> Cards { get; }
        DbSet<CardMember> CardMembers { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    }
}
