using Application.Common.Interfaces;
using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Authorization
{
    public class CardAuthorizationService : ICardAuthorizationService
    {
        private readonly IApplicationDbContext _context;

        public CardAuthorizationService(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CanCreateCardAsync(Guid projectId, Guid boardId, Guid? listId, Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Boards
                .AnyAsync(b =>
                    b.Id == boardId &&
                    b.ProjectId == projectId &&
                    b.Lists.Any(l => listId == null || l.Id == listId) &&
                    b.Project.Members.Any(pm => pm.UserId == userId),
                    cancellationToken);
        }
        public async Task<Dictionary<Guid, Guid>> CanAccessCardsAsync(Guid projectId, Guid boardId, Guid? listId, IEnumerable<Guid> cardIds, Guid userId, CancellationToken cancellationToken)
        {

            if (cardIds == null || !cardIds.Any())
            {
                return new Dictionary<Guid, Guid>();
            }

            var authorizedCards = await _context.Cards
                .Where(c =>
                    cardIds.Contains(c.Id) &&
                    (listId == null || c.ListId == listId) &&
                    c.BoardId == boardId &&
                    c.Board.ProjectId == projectId &&
                    c.Board.Project.Members.Any(pm => pm.UserId == userId))
                .ToDictionaryAsync(c => c.Id, c => c.BoardId, cancellationToken);

            return authorizedCards;
        }

        public async Task<bool> CanAccessCardAsync(Guid projectId, Guid boardId, Guid? listId, Guid cardId, Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Cards
                .AnyAsync(c =>
                    c.Id == cardId &&
                    (listId == null || c.ListId == listId) &&
                    c.BoardId == boardId &&
                    c.Board.ProjectId == projectId &&
                    c.Board.Project.Members.Any(pm => pm.UserId == userId),
                    cancellationToken);
        }
    }
}
