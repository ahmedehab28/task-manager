using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Common;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        private readonly ICurrentUser _currentUser;
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUser currentUser)
            : base(options)
        {
            _currentUser = currentUser;
        }
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
        public DbSet<Board> Boards => Set<Board>();
        public DbSet<CardList> CardLists => Set<CardList>();
        public DbSet<Card> Cards => Set<Card>();
        public DbSet<CardMember> CardMembers => Set<CardMember>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder); // Important for Identity schema
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUser.Id;
            var timeNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = timeNow;
                    if (userId != Guid.Empty)
                        entry.Entity.CreatedBy = userId;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModified = timeNow;
                    if (userId != Guid.Empty)
                        entry.Entity.LastModifiedBy = userId;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return Database.BeginTransactionAsync(cancellationToken);
        }
    }
}
