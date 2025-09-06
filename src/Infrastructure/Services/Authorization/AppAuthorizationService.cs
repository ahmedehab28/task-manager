using Application.Common.Enums;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Services.Authorization
{
    public class AppAuthorizationService(IApplicationDbContext context) : IAppAuthorizationService
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<bool> CanAccessProject(EntityOperations op, Guid projectId, Guid userId, CancellationToken cancellation)
        {
            return await _context.ProjectMembers
                .AnyAsync(
                    pm => pm.ProjectId == projectId &&
                    pm.UserId == userId, cancellation);
        }

        public async Task<bool> CanAccessBoardAsync(EntityOperations op, Guid boardId, Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Boards
                .AnyAsync(b => 
                    b.Id == boardId &&
                    (op == EntityOperations.View || b.BoardType != BoardType.Inbox) &&
                    b.Project.Members
                    .Any(pm =>
                        pm.UserId == userId &&
                        pm.ProjectId == b.ProjectId),
                    cancellationToken);
        }
        public async Task<bool> CanAccessListAsync(EntityOperations op, Guid listId, Guid userId, CancellationToken ct)
        {
            return await _context.CardLists
                .AnyAsync(cl =>
                    cl.Id == listId &&
                    (op == EntityOperations.AddToParent || op == EntityOperations.View || cl.Board.BoardType != BoardType.Inbox) &&
                    cl.Board.Project.Members.Any(pm => pm.UserId == userId));
        }
        public async Task<bool> CanAccessCardAsync(EntityOperations op, Guid cardId, Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Cards
                .AnyAsync(c =>
                    c.Id == cardId &&
                    c.CardList.Board.Project.Members.Any(pm => pm.UserId == userId),
                    cancellationToken);
        }
    }
}
