using Application.Common.Interfaces;
using Application.List.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ListPositionService : IListPositionService
    {
        private readonly IApplicationDbContext _context;
        private const decimal InitialPosition = 170000000m;
        private const decimal PositionIncrement = 16384m;
        public ListPositionService(
            IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<decimal> GetNewListPositionAsync(Guid boardId, CancellationToken cancellationToken = default)
        {
            var boardLists = await _context.CardLists
                .Where(l => l.BoardId == boardId)
                .OrderByDescending(l => l.Position)
                .FirstOrDefaultAsync(cancellationToken);

            return boardLists != null ? boardLists.Position + PositionIncrement : InitialPosition;

        }
        public async Task<decimal> GetMovedListPositionAsync(Guid boardId, Guid? prevListId, Guid? nextListId, Guid listId, CancellationToken cancellationToken = default)
        {
            // Implementation is done on the assumption that the caller has already validated:
            // 1. The target board exists.
            // 2. The provided prevListId and nextListId (if any) exist are not equal and within the target board
            // and are not the same as listId.

            // Step 1: Check if the target board has no lists
            bool boardHasLists = await _context.CardLists
                .AnyAsync(l =>
                    l.BoardId == boardId,
                    cancellationToken);
            if (!boardHasLists)
                return InitialPosition;

            // Step 2: Fetch neighboring lists if IDs are provided
            var neighborIds = new List<Guid>();
            if (prevListId.HasValue)
                neighborIds.Add(prevListId.Value);
            if (nextListId.HasValue)
                neighborIds.Add(nextListId.Value);

            List<CardList> neighborLists = new List<CardList>();

            if (neighborIds.Any())
            {
                neighborLists = await _context.CardLists
                    .Where(l =>
                        neighborIds.Contains(l.Id) &&
                        l.BoardId == boardId)
                    .ToListAsync(cancellationToken);
            }
            var prevList = neighborLists.FirstOrDefault(l => l.Id == prevListId);
            var nextList = neighborLists.FirstOrDefault(l => l.Id == nextListId);

            // Step 3: If no neighboring lists found, throw error (as board should have lists which is ensured in first step)
            if (prevList == null && nextList == null)
                throw new InvalidOperationException("The specified board has lists, but the provided neighboring list IDs do not exist within it.");

            // Step 4: If prevListId is provided and nextListId is not, position after prevList (if it is the last List)
            if (prevList != null && nextList == null)
            {
                var lastList = await _context.CardLists
                    .Where(l =>
                        l.BoardId == boardId)
                    .OrderByDescending(l => l.Position)
                    .FirstOrDefaultAsync(cancellationToken);
                if (lastList!.Id != prevList.Id)
                    throw new InvalidOperationException("Previous List ID is not the last list in this board.");
                return lastList.Position + PositionIncrement;
            }

            // Step 5: If nextListId is provided and prevListId is not, position before nextList (if it is the first List)
            if (nextList != null && prevList == null)
            {
                var firstList = await _context.CardLists
                    .Where(l =>
                        l.BoardId == boardId)
                    .OrderBy(l => l.Position)
                    .FirstOrDefaultAsync(cancellationToken);
                if (firstList!.Id != nextList.Id)
                    throw new InvalidOperationException("Next List ID is not the first list in this board.");
                return firstList.Position - PositionIncrement;
            }

            // Step 6: If both prevListId and nextListId are provided, position between them (They are surely provided at this stage)
            if (prevList!.Position > nextList!.Position)
                    throw new InvalidOperationException("Previous List position must be less than Next List position.");
            
            bool listsBetween = await _context.CardLists
                .Where(l =>
                    l.BoardId == boardId &&
                    l.Position > prevList!.Position &&
                    l.Position < nextList!.Position)
                .AnyAsync(cancellationToken);

            if (listsBetween)
                throw new InvalidOperationException("There are other lists between the specified previous and next lists. Cannot determine a unique position.");

            return (prevList.Position + nextList.Position) / 2;




        }

    }
}
