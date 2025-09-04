using Application.Cards.Services;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CardPositionService : ICardPositionService
    {
        private readonly IApplicationDbContext _context;
        private const decimal InitialPosition = 170000000m;
        private const decimal PositionIncrement = 16384m;

        public CardPositionService(
            IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<decimal> GetNewCardPositionAsync(Guid boardId, Guid? listId, CancellationToken cancellationToken = default)
        {
            if (listId == null)
            {

                var firstInboxCard = await _context.Cards
                    .Where(c =>
                        c.ListId == null &&
                        c.BoardId == boardId)
                    .OrderBy(c => c.Position)
                    .FirstOrDefaultAsync(cancellationToken);

                return firstInboxCard != null ? firstInboxCard.Position - PositionIncrement : InitialPosition;
            }
            else
            {
                var lastListCard = await _context.Cards
                    .Where(c =>
                        c.ListId == listId &&
                        c.BoardId == boardId)
                    .OrderByDescending(c => c.Position)
                    .FirstOrDefaultAsync(cancellationToken);

                return lastListCard != null ? lastListCard.Position + PositionIncrement : InitialPosition;
            }
        }

        public async Task<decimal> GetMovedCardPositionAsync(Guid boardId, Guid? targetListId, Guid? prevCardId, Guid? nextCardId, Guid CardId, CancellationToken cancellationToken = default)
        {
            // Implementation is done on the assumption that the caller has already validated:
            // 1. The target list exists within the specified board.
            // 2. The provided prevCardId and nextCardId (if any) exist are not equal and within the target list
            // and are not the same as CardId.



            // Step 1: Check if the target list is empty 
            bool listHasCards = await _context.Cards
                .AnyAsync(c =>
                    c.ListId == targetListId &&
                    c.BoardId == boardId,
                    cancellationToken);
            if (!listHasCards)
                return InitialPosition;

            // Step 2: Fetch neighboring cards if IDs are provided
            var neighborIds = new List<Guid>();
            if (prevCardId.HasValue) neighborIds.Add(prevCardId.Value);
            if (nextCardId.HasValue) neighborIds.Add(nextCardId.Value);

            List<Card> neighborCards = new List<Card>();

            if (neighborIds.Any())
            {
                neighborCards = await _context.Cards
                    .Where(c => c.BoardId == boardId && c.ListId == targetListId && neighborIds.Contains(c.Id))
                    .ToListAsync(cancellationToken);
            }

            var prevCard = prevCardId.HasValue ? neighborCards.FirstOrDefault(c => c.Id == prevCardId.Value) : null;
            var nextCard = nextCardId.HasValue ? neighborCards.FirstOrDefault(c => c.Id == nextCardId.Value) : null;


            // Step 3: If no neighboring cards found, throw error (as list isn't empty from first step)
            if (prevCard == null && nextCard == null)
                throw new InvalidOperationException("No Neighboring Card IDs provided and List isn't empty");

            // Step 4: If prevCardId is provided and nextCardId is not, position after prevCard (if it is the last card)
            if (prevCard != null && nextCard == null)
            {
                var lastCard = await _context.Cards
                    .Where(c =>
                        c.ListId == targetListId &&
                        c.BoardId == boardId)
                    .OrderByDescending(c => c.Position)
                    .FirstOrDefaultAsync(cancellationToken);
                if (lastCard!.Id != prevCard.Id)
                    throw new InvalidOperationException("Previous Card ID is not the last card in the list");
                return lastCard.Position + PositionIncrement;
            }

            // Step 5: If nextCardId is provided and prevCardId is not, position before nextCard (if it is the first card)
            if (prevCard == null && nextCard != null)
            {
                var firstCard = await _context.Cards
                    .Where(c =>
                        c.ListId == targetListId &&
                        c.BoardId == boardId)
                    .OrderBy(c => c.Position)
                    .FirstOrDefaultAsync(cancellationToken);
                if (firstCard!.Id != nextCard.Id)
                    throw new InvalidOperationException("Next Card ID is not the first card in the list");
                return firstCard.Position - PositionIncrement;
            }

            // Step 6: If both prevCardId and nextCardId are provided, position between them (if they are adjacent)

            if (prevCard!.Position > nextCard!.Position)
                throw new InvalidOperationException("Previous Card must be positioned before Next Card");
            // Check if they are adjacent
            var cardsBetween = await _context.Cards
                .Where(c =>
                    c.ListId == targetListId &&
                    c.BoardId == boardId &&
                    c.Position > prevCard.Position &&
                    c.Position < nextCard.Position)
                .AnyAsync(cancellationToken);
            if (cardsBetween)
                throw new InvalidOperationException("Previous and Next Cards are not adjacent");
            return (prevCard.Position + nextCard.Position) / 2;
            
        }
    }
}
